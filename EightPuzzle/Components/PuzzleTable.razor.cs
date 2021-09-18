﻿using EightPuzzle.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EightPuzzle.Components
{
    public partial class PuzzleTable : ComponentBase
    {
        [Parameter]
        public bool AllowShuffle { get; set; } = false;
        [Parameter]
        public bool AllowDrag { get; set; } = true;
        [Parameter]
        public List<List<Tile>> Rows { get; set; }
        [Parameter]
        public EventCallback<List<List<Tile>>> RowsChanged { get; set; }

        private int CurrentIndex;
        private int CurrentRow;

        private void ResetSelectedValues()
        {
            CurrentRow = 0;
            CurrentIndex = -1;
        }

        private void ShuffleTiles()
        {
            Random rng = new Random();
            int i, XSortedDirection, YSortedDirection, newRow, newIndex, currentRow, currentIndex;

            for(i=0; i<100; i++)
            {
                currentRow = 0;
                currentIndex = -1;


                for (; currentIndex < 0 && currentRow < Rows.Count; currentRow++)
                {
                    var row = Rows.ElementAt(currentRow);
                    currentIndex = row.FindIndex(t => t.Value == 0);
                }
                currentRow -= 1;

                #region Finding new X Position
                newIndex = currentIndex;

                // <- Even | Odd ->
                XSortedDirection = rng.Next(1, 10);
                if ((XSortedDirection % 2) == 0 && currentIndex + 1 < Rows.ElementAt(currentRow).Count)
                {
                    newIndex += 1;
                }
                else if (currentIndex - 1 >= 0)
                {
                    newIndex -= 1;
                }
                #endregion

                #region Finding new Y Position
                newRow = currentRow;

                // /\ Even | Odd \/
                YSortedDirection = rng.Next(1, 10);
                if ((YSortedDirection % 2) == 0 && currentRow + 1 < Rows.Count)
                {
                    newRow += 1;
                } else if (currentRow - 1 >= 0) {
                    newRow -= 1;
                }
                #endregion

                #region Swap Tiles
                var currentTile = Rows.ElementAt(currentRow)
                    .ElementAt(currentIndex);

                var destinationTile = Rows.ElementAt(newRow)
                    .ElementAt(newIndex);

                Rows[newRow][newIndex] = currentTile;
                Rows[currentRow][currentIndex] = destinationTile;
                #endregion
            }

            StateHasChanged();
        }

        private void StartDrag(Tile tile)
        {
            ResetSelectedValues();

            for (; CurrentIndex < 0 && CurrentRow < Rows.Count; CurrentRow++)
            {
                var row = Rows.ElementAt(CurrentRow);
                CurrentIndex = row.FindIndex(t => t.Value == tile.Value);
            }
        }

        void Drop(Tile destinationTile)
        {
            if (destinationTile is not null && CurrentRow > 0)
            {
                int destinationIndex = -1, destinationRow = 0;
                for (; destinationIndex < 0 && destinationRow < Rows.Count; destinationRow++)
                {
                    var row = Rows.ElementAt(destinationRow);
                    destinationIndex = row.FindIndex(t => t.Value == destinationTile.Value);
                }

                var currentTile = Rows.ElementAt(CurrentRow - 1)
                    .ElementAt(CurrentIndex);

                Rows[CurrentRow - 1][CurrentIndex] = destinationTile;
                Rows[destinationRow - 1][destinationIndex] = currentTile;

                ResetSelectedValues();
                StateHasChanged();
            }
        }
    }
}
