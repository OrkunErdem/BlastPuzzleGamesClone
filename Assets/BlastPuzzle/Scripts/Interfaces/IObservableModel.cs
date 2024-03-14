using System;

namespace BlastPuzzle.Scripts.Interfaces
{
    public interface IObservableModel
    {
        event Action PropertyChanged;
    }
}