using System;
using UnityEngine;
using UnityEngine.Events;

public class CollectionManager : MonoBehaviour
{
    [Serializable]
    public class CollectibleGamePiece
    {
        public GameObject prefab;
        public bool collected;
        public bool owned;
    }

    [Serializable]
    public class GamePieceSeries
    {
        public CollectibleGamePiece[] gamePiece;
        public bool completedOnce = false;
    }

    [System.Serializable]
    public class PieceCollectedEvent : UnityEvent<int> { }

    public GamePieceSeries[] pieces;
    public int numberOfSeries = 2;
    public int piecesPerSeries = 6;

    public PieceCollectedEvent pieceCollectedEvent;

    public void AddCollectedPiece(int series, int numInSeries)
    {
        pieces[series-1].gamePiece[numInSeries-1].collected = true;
        pieceCollectedEvent.Invoke(((series-1) * 6) + numInSeries - 1);
    }

    public GameObject GetPrefab(int series, int numInSeries)
    {
        return pieces[series - 1].gamePiece[numInSeries - 1].prefab;
    }

    public bool IsPieceOwned(int series, int numInSeries)
    {
        return pieces[series - 1].gamePiece[numInSeries - 1].owned;
    }

    public bool IsPieceCollectedOrOwned(int series, int numInSeries)
    {
        return pieces[series-1].gamePiece[numInSeries-1].collected || pieces[series-1].gamePiece[numInSeries-1].owned;
    }

    public bool IsSetCollected(int series)
    {
        bool result = true;
        for (int i = 0; i < piecesPerSeries; i++)
        {
            result &= IsPieceCollectedOrOwned(series, i + 1);
        }
        return result;
    }

    public bool IsSetFlaggedCollectedOnce(int series)
    {
        return pieces[series - 1].completedOnce;
    }

    public void SetFlagSetCollectedOnce(int series, bool newFlag)
    {
        pieces[series - 1].completedOnce = newFlag;
    }

    public void PersistCollectedPieces()
    {
        for (int i = 0; i < numberOfSeries; i++)
        {
            for (int j = 0; j < piecesPerSeries; j++)
            {
                pieces[i].gamePiece[j].owned |= pieces[i].gamePiece[j].collected;
            }
        }
    }

    public void DiscardCollectedPieces()
    {
        for (int i = 0; i < numberOfSeries; i++)
        {
            for (int j = 0; j < piecesPerSeries; j++)
            {
                pieces[i].gamePiece[j].collected = false;
            }
        }
    }

}
