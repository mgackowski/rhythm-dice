using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GamePieceSeries[] pieces;
    public int numberOfSeries = 2;
    public int piecesPerSeries = 6;

    public void AddCollectedPiece(int series, int numInSeries)
    {
        pieces[series-1].gamePiece[numInSeries-1].collected = true;
    }

    public bool IsPieceCollectedOrOwned(int series, int numInSeries)
    {
        return pieces[series-1].gamePiece[numInSeries-1].collected || pieces[series-1].gamePiece[numInSeries-1].owned;
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
