using System.Diagnostics;
using FNFNewBot.Dto;

namespace FNFNewBot.KeyboardSimulator;

public interface IKeyboardSimulator
{
    void PressKey(int keyCode, double? length, int direction, int pressTime, int holdTime, Stopwatch stopwatch, int salt, List<KeyType> keyTypes);
}