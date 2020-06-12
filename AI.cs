using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace WindowsFormsApplication309
{
    //метод мінімакса
    class AI
    {
        public Move GetBestMove(Game game)
        {
            //шукаємо вільні комірки
            if(!GetEmptyCells(game).Any())
                return null;//якщо немає вільних комірок

            //вибираемо кращий хід з вільних комірок
            return GetEmptyCells(game).Select(p => GetMoveFitness(game, p)).Max();
        }

        public Move GetMoveFitness(Game game, Point p)
        {
            var res = new Move() {P = p};
            //імітуємо хід
            var g = game.Clone();
            g.MakeMove(p);

            //якщо виграли вертаємо 1
            if (g.Winned)
                res.Fitness = 1f;
            else
            {
                //вибираємо кращий варіант для ходу супротивника
                var best = GetBestMove(g);
                if (best != null)
                    res.Fitness = -best.Fitness; //вертаєм гіршу для нас оцінку
            }

            return res;
        }

        IEnumerable<Point> GetEmptyCells(Game game)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (game.Items[i, j] == FieldState.Empty)
                        yield return new Point(i, j);
        }
    }

    //Хід
    class Move : IComparable<Move>
    {
        public Point P;
        public float Fitness;

        public int CompareTo(Move other)
        {
            var res = Fitness.CompareTo(other.Fitness);
            if (res == 0 && P.X == 1 && P.Y == 1) return 1;//даємо перевагу ходу в центр
            return res;
        }
    }
}
