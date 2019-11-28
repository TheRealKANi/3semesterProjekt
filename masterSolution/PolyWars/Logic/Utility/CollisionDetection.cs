using PolyWars.API;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace PolyWars.Logic.Utility {
    class CollisionDetection {
        public static void resourceCollisionDetection() {

            List<IResource> toRemove = new List<IResource>();

            foreach( IResource resource in GameController.Resources ) {
                if( resource.Polygon.RenderedGeometry.Bounds.IntersectsWith( GameController.Player.Shape.Polygon.RenderedGeometry.Bounds ) && 
                        !resource.Polygon.Visibility.Equals( Visibility.Collapsed ) ) {
                    GameController.Player.CurrencyWallet += resource.ResourceValue;
                    toRemove.Add(resource);
                    //resource.Polygon.Visibility = Visibility.Hidden;
                }
            }
            foreach( IResource resource in toRemove ) {
                GameController.Resources.Remove( resource );
                resource.Polygon.Visibility = Visibility.Collapsed;
            }
            toRemove.Clear();
        }
    }
}
