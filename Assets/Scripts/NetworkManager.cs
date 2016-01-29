using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

	private const string PLAYER_ID_PREFIX = "Player ";
	private static Dictionary<string, Player> players = new Dictionary<string, Player> ();

	public static void RegisterPlayer(string _netID, Player _player) {
		string _playerID = PLAYER_ID_PREFIX + _netID;
		players.Add (_playerID, _player);
		_player.transform.name = _playerID;
	}

	public static void UnRegisterPlayer (string _playerID) {
		players.Remove (_playerID);
	}
}
