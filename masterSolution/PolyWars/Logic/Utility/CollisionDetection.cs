using PolyWars.API;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace PolyWars.Logic.Utility {
    class CollisionDetection {
        public static void resourceCollisionDetection() {
            foreach( IResource resource in GameController.Resources ) {
                if( resource.Polygon.RenderedGeometry.Bounds.IntersectsWith( GameController.Player.Shape.Polygon.RenderedGeometry.Bounds ) && !resource.Polygon.Visibility.Equals( Visibility.Hidden ) ) {
                    GameController.Player.CurrencyWallet += resource.ResourceValue;
                    resource.Polygon.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
