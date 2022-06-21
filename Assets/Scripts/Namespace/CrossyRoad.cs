﻿using Photon.Pun;
using System;
using System.Collections.Generic;

namespace Crossyroad
{
    public enum Panel
    {
        HOME, PLAYER_LIST, FINDING_OPPONENT, TYPED_LOBBY, CHALLENGE_REQUEST,
        GAME_OVER, COUNTDOWN, CHALLENGE_RESPONSE, CREATE_ROOM, TOSS,
        IN_GAME_CHAT, PAUSE, SETTINGS, CHOOSE_ATTRIBUTE, LOADING, CARD_SELECTION,
        ROUND_OVER, LEAVE_LOBBY, PLAYER_GAME_INFO, ATTRIBUTE, ROUND_WINNER, ROUND_WON, ROUND_LOST, ROUND_TIE, WAITING_FOR_OPPONENT,
        OPPONENT_LEFT_ROOM, ACTION_CARD,
        CARD_INFO_LEFT, CARD_INFO_RIGHT,
        CHALLENGE_SENT,
        START_GAME, START_ROUND, ATTRIBUTE_SELECTED, ROUND_STATUS, OPPONENT__NOT_REAY,
        WAIT_SELECT_ATTRIBUTE, CHALLENGE_WAIT, ATTRIBUTE_ALREADY_SELECTED,
        TOURNEMENT_SCREEN,
        LUCKY_EVENT_SCREEN,
        LUCKY_EVENT_USED_PLAYER_SCREEN,
        GAMBLE_SCREEN,
        PLAYING_GAMBLE,
        GAMBLE_SELECTING_VIEW,
        LOGIN_VIEW,
        PROGRASS_BAR_VIEW,
        NONE
    }


    public enum RoomType { LOBBY, RANDOM, CHALLENGE, TOURNEMENT }


    public enum PlayerStatus
    {
        NONE, CHALLENGE, PHOTON_LOBBY, LOBBY_ROOM,
        RANDOM_JOIN, RANDOM_JOIN_FAILED, PHOTON_LOBBY_FAILED,
        LEAVE_ROOM, OPPONENT_JOINED, OPPONENT_LEFT,
        JOIN_ROOM,
        JOIN_CHALLENGE, CREATE_CHALLENGE, JOIN_CHALLENGE_FAILED,
        CREATE_ROOM_FAILED,
        IN_GAME,
        SHUFFLED,
        RANDOM_JOIN_RECONNECT,
        RELOAD_SCENE,
        ON_MASTER_CLENT_SWITCH,
        TOURNEMENT_MATCH,
            MASTER_CLIENT_SWITCH
    }


    public enum RaiseEventType
    {
        NONE,
        ACKNOWLEDGE_SCENE_LOAD,
        START_GAME,
        PLAYER_SPAWN,
        ALL_PLAYER_SPAWN,

        RANDOM_PLAYER_READY,
        CHALLENGE_REQUEST,
        CHALLENGE_RESPONSE,
        DECKS_READY,
        START_ROUND,
        OPPONENT_DECKS_DOWNLOADED,
        TURN,
        ATTRIBUTE_SELECTED,
        CHALLENGE_ROOM_CREATED,
        ACKNOWLEDGE_ROUND_END,
        SET_TURN,
        FORCE_CARD_TURN,
        CARD_FIGHT_COMPLETED,
        END_ROUND,
        ACTION_CARD,
        ACKNOWLEDGE_ACTION_CARD,
        ACKNOWLEDGE_DECK_DISPLAYED,
        OPPONENT_NOTREADY,
        CHALLENGE_TIMER_EXPIRED,
        PLAYER_ON_BACKGROUND,
        RPC_REQUESTMASTERCLIENT,
        RPC_RECIEVEPING,
        RPC_MASTERCLIENTGRANTED,
        ATTRIBUTE_SELECT_TIMER,
        LUCKY_EVENT_SELECTED,
        FIND_OPPONENT_TIMER_START,
        FIND_OPPONENT_TIMER_STOP
    }

}