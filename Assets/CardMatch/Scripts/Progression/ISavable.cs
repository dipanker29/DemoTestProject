using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardMatch.Progress
{
    public interface ISavable
    {
        void Save(ProgressionData data);
        ProgressionData Load();
    }
}
