using System;
using UnityEngine;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Multiplayer;

public class SessionManager : MonoBehaviour
{
	public string defaultSessionId = "default";

	async void Start()
    {
	    await SignIn();

	    // Does not work on WebGL!
	    // var joinedSessionIds = await MultiplayerService.Instance.GetJoinedSessionIdsAsync();
	    // var isAlreadyPartOfSession = joinedSessionIds.Contains(defaultSessionId);

		await CreateOrJoinSession(defaultSessionId);
    }

	private async Task<ISession> RejoinSession(string sessionId)
	{
		Debug.Log($"Rejoining Session {sessionId}...");
		var session = await MultiplayerService.Instance.ReconnectToSessionAsync(sessionId);
		Debug.Log($"Session {session.Id} rejoined! Join code: {session.Code}");
		return session;
	}

	private async Task SignIn()
    {
	    await UnityServices.InitializeAsync();
	    await AuthenticationService.Instance.SignInAnonymouslyAsync();
	    Debug.Log($"Sign in anonymously succeeded! PlayerID: {AuthenticationService.Instance.PlayerId}");
    }

    private async Task<ISession> CreateOrJoinSession(string sessionId)
    {
	    Debug.Log($"Creating/Joining Session {sessionId}...");
        var options = new SessionOptions
        {
    	    MaxPlayers = 6
        }.WithRelayNetwork();

        try
        {
	        var session = await MultiplayerService.Instance.CreateOrJoinSessionAsync(sessionId, options);
	        Debug.Log($"Session {session.Id} created! Join code: {session.Code}");
	        return session;
        }
        catch (SessionException e)
        {
	        Debug.Log(e.Error.ToString());
	        return await RejoinSession(sessionId);
        }
    }
}
