using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardMatch
{
    public enum GameState
    {
        MainMenu,
        Gameplay,
        GameOver
    }

    public enum Difficulty
    {
        TwoXTwo,
        ThreeXThree,
        FourXFour,
        FourXFive,
        FiveXSix
    }

    public enum SoundFx
    {
        None,
        Flip,
        Match,
        Mismatch,
        GameOver
    }
}
