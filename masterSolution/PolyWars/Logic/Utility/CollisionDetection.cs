using PolyWars.API;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace PolyWars.Logic.Utility {
    class CollisionDetection {
        public static void resourceCollisionDetection( List<IResource> resources, IPlayer player ) {
            // Start iteration from second child in canvas, player is the first child.
            foreach( IResource resource in resources.GetRange( 0, resources.Count - 1 ) ) {

                if( resource.Polygon.RenderedGeometry.Bounds.IntersectsWith( player.Shape.Polygon.RenderedGeometry.Bounds ) && !resource.Polygon.Visibility.Equals( Visibility.Hidden ) ) {
                    player.CurrencyWallet += resource.ResourceValue;
                    resource.Polygon.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
