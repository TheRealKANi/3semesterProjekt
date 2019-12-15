using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolyWars.API.Model;
using PolyWars.API.Model.Interfaces;
using PolyWars.Client.Logic;
using PolyWars.Client.Model;
using System.Windows;
using System.Windows.Media;

namespace PolyWars.ServerClasses.Tests {
    [TestClass()]
    public class MoveStrategyTests {
        [TestMethod()]
        public void MoveTest() {
            UnitTestUIDispatcher dispatcher = new UnitTestUIDispatcher();
            Point startPoint = new Point(100, 100);
            MoveStrategy ms = new MoveStrategy();
            IRay ray = new Ray("1", startPoint, 0);
            IRenderable renderable = new Renderable(Colors.Black, Colors.White, 1, 15, 15, 3);
            Shape shape = new Shape("1", ray, renderable, new RenderWithHeaderStrategy());
            Moveable mv = new Moveable(15, 15, 0, 0, shape, new MoveOpponentStrategy());
            ms.Move(mv, 1);
            mv.Shape.Ray.Angle += 90;

            ms.Move(mv, 1);
            mv.Shape.Ray.Angle += 90;

            ms.Move(mv, 1);
            mv.Shape.Ray.Angle += 90;
            ms.Move(mv, 1);

            Assert.IsTrue(mv.Shape.Ray.CenterPoint.Equals(startPoint));
        }
    }
}