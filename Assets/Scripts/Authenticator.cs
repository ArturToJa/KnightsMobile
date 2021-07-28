using Mirror;
using System;
using System.Collections;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;

public class Authenticator : NetworkAuthenticator
{
    public const string EMAIL_PATTERN = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
    public const string USERNAME_AND_DISCRIMINATOR_PATTERN = @"^[a-zA-Z0-9]{4,20}#[0-9]{4}$";
    public const string USERNAME_PATTERN = @"^[a-zA-Z0-9]{4,20}$";
    public const string RANDOM_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public DatabaseReference DBReference;

    // this is set if authentication fails to prevent garbage AuthRequestMessage spam
    bool ServerAuthFailed;

    #region Messages

    public struct AuthRequestMessage : NetworkMessage
    {
        public string authEmail;
        public string authUsername;
        public string authPassword;
        public bool isRegister;
    }

    public struct AuthResponseMessage : NetworkMessage
    {
        public byte code;
        public string message;
    }

    #endregion

    #region Server

    /// <summary>
    /// Called on server from StartServer to initialize the Authenticator
    /// <para>Server message handlers should be registered in this method.</para>
    /// </summary>
    public override void OnStartServer()
    {
        // register a handler for the authentication request we expect from client
        NetworkServer.RegisterHandler<AuthRequestMessage>(OnAuthRequestMessage, false);
    }

    /// <summary>
    /// Called on server from StopServer to reset the Authenticator
    /// <para>Server message handlers should be registered in this method.</para>
    /// </summary>
    public override void OnStopServer()
    {
        // unregister the handler for the authentication request
        NetworkServer.UnregisterHandler<AuthRequestMessage>();
    }

    /// <summary>
    /// Called on server from OnServerAuthenticateInternal when a client needs to authenticate
    /// </summary>
    /// <param name="conn">Connection to client.</param>
    public override void OnServerAuthenticate(NetworkConnection conn)
    {
        // do nothing...wait for AuthRequestMessage from client
    }

    /// <summary>
    /// Called on server when the client's AuthRequestMessage arrives
    /// </summary>
    /// <param name="conn">Connection to client.</param>
    /// <param name="msg">The message payload</param>
    public void OnAuthRequestMessage(NetworkConnection conn, AuthRequestMessage msg)
    {
        if(msg.isRegister)
        {
            RegisterUser(conn, msg);
        }
        else
        {
            LoginUser(conn, msg);
        }
    }

    void LoginUser(NetworkConnection conn, AuthRequestMessage msg)
    {
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(msg.authEmail, msg.authPassword).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                AuthError error = (AuthError)e.ErrorCode;

                DenyAuthentication(conn, error.ToString());
            }

            if (task.IsCompleted)
            {
                if (!task.Result.IsEmailVerified)
                {
                    DenyAuthentication(conn, "Email not verified");
                }
                else
                {
                    // create and send msg to client so it knows to proceed
                    AuthResponseMessage authResponseMessage = new AuthResponseMessage
                    {
                        code = 100,
                        message = "Success"
                    };

                    conn.Send(authResponseMessage);
                    conn.authenticationData = task.Result;

                    // Accept the successful authentication
                    ServerAccept(conn);
                }
            }
        });
    }

    void RegisterUser(NetworkConnection conn, AuthRequestMessage msg)
    {
        if (msg.authUsername != "")
        {
            FirebaseManager.instance.IsUsernameAvailable(msg.authUsername.ToLower(), result => 
            {
                if (result)
                {
                    FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(msg.authEmail, msg.authPassword).ContinueWith(task => 
                    {
                        if (task.IsFaulted || task.IsCanceled)
                        {
                            Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                            AuthError error = (AuthError)e.ErrorCode;

                            DenyAuthentication(conn, error.ToString());
                        }
                        if (task.IsCompleted)
                        {
                            var result = task.Result.SendEmailVerificationAsync(); // handle email resend if anything goes wrong
                            FirebaseManager.instance.AddUsername(task.Result.UserId, msg.authUsername.ToLower());
                            UserProfile profile = new UserProfile();
                            profile.DisplayName = msg.authUsername;
                            task.Result.UpdateUserProfileAsync(profile); // handle error if anything goes wrong
                            DenyAuthentication(conn, "Success");
                        }
                    });
                }
                else
                {
                    DenyAuthentication(conn, "Username not available");
                }
            });
        }
        else
        {
            DenyAuthentication(conn, "Username field is empty");
        }
    }

    IEnumerator DelayedDisconnect(NetworkConnection conn, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        // Reject the unsuccessful authentication
        ServerReject(conn);
    }

    void DenyAuthentication(NetworkConnection conn, string error)
    {
        // create and send msg to client so it knows to disconnect
        AuthResponseMessage authResponseMessage = new AuthResponseMessage
        {
            code = 200,
            message = error
        };

        conn.Send(authResponseMessage);

        // must set NetworkConnection isAuthenticated = false
        conn.isAuthenticated = false;

        // disconnect the client after 1 second so that response message gets delivered
        if (!ServerAuthFailed)
        {
            // set this false so this coroutine can only be started once
            ServerAuthFailed = true;

            StartCoroutine(DelayedDisconnect(conn, 1.0f));
        }
    }

    #endregion

    #region Client

    /// <summary>
    /// Called on client from StartClient to initialize the Authenticator
    /// <para>Client message handlers should be registered in this method.</para>
    /// </summary>
    public override void OnStartClient()
    {
        // register a handler for the authentication response we expect from server
        NetworkClient.RegisterHandler<AuthResponseMessage>((Action<AuthResponseMessage>)OnAuthResponseMessage, false);
    }

    /// <summary>
    /// Called on client from StopClient to reset the Authenticator
    /// <para>Client message handlers should be unregistered in this method.</para>
    /// </summary>
    public override void OnStopClient()
    {
        // unregister the handler for the authentication response
        NetworkClient.UnregisterHandler<AuthResponseMessage>();
    }

    /// <summary>
    /// Called on client from OnClientAuthenticateInternal when a client needs to authenticate
    /// </summary>
    public override void OnClientAuthenticate()
    {
        UIManager manager = UIManager.instance;
        AuthRequestMessage authRequestMessage = new AuthRequestMessage
        {
            authEmail = manager.email,
            authUsername = manager.username,
            authPassword = manager.password,
            isRegister = manager.isRegister
        };

        NetworkClient.connection.Send(authRequestMessage);
    }

    [Obsolete("Call OnAuthResponseMessage without the NetworkConnection parameter. It always points to NetworkClient.connection anyway.")]
    public void OnAuthResponseMessage(NetworkConnection conn, AuthResponseMessage msg) => OnAuthResponseMessage(msg);

    /// <summary>
    /// Called on client when the server's AuthResponseMessage arrives
    /// </summary>
    /// <param name="msg">The message payload</param>
    public void OnAuthResponseMessage(AuthResponseMessage msg)
    {
        if (msg.code == 100)
        {
            // Debug.LogFormat(LogType.Log, "Authentication Response: {0}", msg.message);

            // Authentication has been accepted
            ClientAccept();
        }
        else
        {
            Debug.LogError($"Authentication Response: {msg.message}");

            // Authentication has been rejected
            ClientReject();
        }
    }

    #endregion
}
