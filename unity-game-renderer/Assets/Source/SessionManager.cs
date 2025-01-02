using UnityEngine;
using Unity.Services.Core;
using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Multiplayer;
using UnityEngine.Serialization;

public class SessionManager : MonoBehaviour
{
	public string defaultSessionId = "default";

	async void Start()
    {
	    await SignIn();

	    var session = await IsAlreadyPartOfSession(defaultSessionId)
		    ? await RejoinSession(defaultSessionId)
			: await CreateOrJoinSession(defaultSessionId);
    }

	private async Task<ISession> RejoinSession(string sessionId)
	{
		var session = await MultiplayerService.Instance.ReconnectToSessionAsync(sessionId);
		Debug.Log($"Session {session.Id} rejoined! Join code: {session.Code}");
		return session;
	}

	private async Task<bool> IsAlreadyPartOfSession(string sessionId)
	{
		return (await MultiplayerService.Instance.GetJoinedSessionIdsAsync()).Contains(sessionId);
	}

	private async Task SignIn()
    {
	    await UnityServices.InitializeAsync();
	    await AuthenticationService.Instance.SignInAnonymouslyAsync();
	    Debug.Log($"Sign in anonymously succeeded! PlayerID: {AuthenticationService.Instance.PlayerId}");
    }

    private async Task<ISession> CreateOrJoinSession(string sessionId)
    {
        var options = new SessionOptions
        {
    	    MaxPlayers = 4
        }.WithRelayNetwork();

        var session = await MultiplayerService.Instance.CreateOrJoinSessionAsync(sessionId, options);
        Debug.Log($"Session {session.Id} created! Join code: {session.Code}");
        return session;
    }
}