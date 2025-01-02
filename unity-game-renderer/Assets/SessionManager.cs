using UnityEngine;
using Unity.Services.Core;
using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Multiplayer;

public class SessionManager : MonoBehaviour
{
	public string sessionId = "default";

	async void Start()
    {
	    await SignIn();
	    await CreateOrJoinSession();
    }

    private async Task CreateOrJoinSession()
    {
	    var options = new SessionOptions
	    {
		    MaxPlayers = 4
	    }.WithRelayNetwork();

	    var session = await MultiplayerService.Instance.CreateOrJoinSessionAsync(sessionId, options);
	    Debug.Log($"Session {session.Id} created! Join code: {session.Code}");
    }

    private async Task SignIn()
    {
	    await UnityServices.InitializeAsync();
	    await AuthenticationService.Instance.SignInAnonymouslyAsync();
	    Debug.Log($"Sign in anonymously succeeded! PlayerID: {AuthenticationService.Instance.PlayerId}");
    }
}
