using EightPuzzle.Models;
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
        private int CurrentIndex;
        private int RowNumber;
        private List<List<Tile>> Rows;

        public PuzzleTable()
        {
            Rows = new List<List<Tile>>
            {
                new List<Tile>
                {
                    new Tile() { Value = 1, Color = "bisque" },
                    new Tile() { Value = 2, Color = "aquamarine" },
                    new Tile() { Value = 3, Color = "darksalmon" }
                },
                new List<Tile>
                {
                    new Tile() { Value = 4, Color = "cadetblue" },
                    new Tile() { Value = 5, Color = "gainsboro" },
                    new Tile() { Value = 6, Color = "greenyellow" }
                },
                new List<Tile>
                {
                    new Tile() { Value = 7, Color = "goldenrod" },
                    new Tile() { Value = 8, Color = "palegreen" },
                    new Tile() { Value = 0, Color = "deepskyblue" }
                }
            };
        }

        private void ResetSelectedValues()
        {
            RowNumber = 0;
            CurrentIndex = -1;
        }

        private void ShuffleTiles()
        {
            for(int i=0; i<100; i++)
            {
                for (; CurrentIndex < 0 && RowNumber < Rows.Count; RowNumber++)
                {
                    var row = Rows.ElementAt(RowNumber);
                    CurrentIndex = row.FindIndex(t => t.Value == 0);
                }
            }
        }

        private void StartDrag(Tile tile)
        {
            ResetSelectedValues();

            for (; CurrentIndex < 0 && RowNumber < Rows.Count; RowNumber++)
            {
                var row = Rows.ElementAt(RowNumber);
                CurrentIndex = row.FindIndex(t => t.Value == tile.Value);
            }
        }

        void Drop(Tile destinationTile)
        {
            if (destinationTile is not null && RowNumber > 0)
            {
                var currentTile = Rows.ElementAt(RowNumber - 1)
                    .ElementAt(CurrentIndex);

                int currentTileValue = currentTile.Value;
                string currentTileColor = currentTile.Color;


                int destinationTileValue = destinationTile.Value;
                string destinationTileColor = destinationTile.Color;


                destinationTile.Value = currentTileValue;
                destinationTile.Color = currentTileColor;

                currentTile.Value = destinationTileValue;
                currentTile.Color = destinationTileColor;

                ResetSelectedValues();
                StateHasChanged();
            }
        }
    }
}
