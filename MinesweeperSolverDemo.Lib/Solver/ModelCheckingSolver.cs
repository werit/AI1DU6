using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinesweeperSolverDemo.Lib.Objects;

namespace MinesweeperSolverDemo.Lib.Solver
{
    public class ModelCheckingSolver : SingleGameSolver
    {
        List<Panel> fringe;

        public override void Deliberate()
        {
			fringe = this.Board.Panels.Where(p => (!p.IsRevealed && !p.IsFlagged && Board.GetNeighbors(p.X, p.Y).Any(q => q.IsRevealed))).ToList();
			//TODO continue
            var panels = fringe.Take(5);
            foreach (var panel in panels)
            {
                panel.beliefState = searchState.tryMine;
                if (!isThereAModel(panel))
                {
                    Board.RevealPanel(panel.X,panel.Y);
                    return;
                }
                panel.beliefState = searchState.notDecided;
            }
            

            base.Deliberate();
        }

        private bool isThereAModel(Panel justChangedPanel)
        {
			int startPoint = fringe.IndexOf(justChangedPanel);
            return isThereAModelRecur(startPoint + 1, startPoint);
        }

        private bool isThereAModelRecur(int i, int startPoint)
        {
            if (i == startPoint)
                return true;

            if (i >= fringe.Count)
                return isThereAModelRecur(0, startPoint);
            bool isModelFound;
            fringe[i].beliefState = searchState.tryMine;
            if (isCorrect(fringe[i]))
            {
                isModelFound = isThereAModelRecur(i + 1, startPoint);
                if (isModelFound)
                {
                    fringe[i].beliefState = searchState.notDecided;
                    return true;
                }
            }

            fringe[i].beliefState = searchState.tryNotMine;
            if (isCorrect(fringe[i]))
            {
                isModelFound = isThereAModelRecur(i + 1, startPoint);
                if (isModelFound)
                {
                    fringe[i].beliefState = searchState.notDecided;
                    return true;
                }
            }
            fringe[i].beliefState = searchState.notDecided;
            return false;
        }

        private bool isCorrect(Panel p)
        {
            var neighbours = Board.GetNeighbors(p.X, p.Y);
            foreach (var item in neighbours)
            {
                if (!item.IsRevealed || item.IsFlagged)
                    continue;
                var minPossibleMinesAround = Board.GetNeighbors(item.X, item.Y).Where(t => t.IsFlagged || t.beliefState == searchState.tryMine).Count();
                var maxPossibleMinesAround = Board.GetNeighbors(item.X, item.Y).Where(t => t.IsFlagged || (!t.IsRevealed && t.beliefState != searchState.tryNotMine)).Count();
                if ((minPossibleMinesAround > item.AdjacentMines) || (maxPossibleMinesAround < item.AdjacentMines))
                    return false;
            }
            return true;
        }

        public ModelCheckingSolver(Random rand, Visualizer vis, GameBoard board)
            : base(rand, vis, board)
        {

        }

        public ModelCheckingSolver(Random rand, Visualizer vis)
            : base(rand, vis)
        {

        }
    }

}
