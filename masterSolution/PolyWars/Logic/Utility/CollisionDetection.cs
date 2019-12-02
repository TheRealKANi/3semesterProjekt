using PolyWars.API;
using System.Collections.Generic;
using System.Windows;

namespace PolyWars.Logic.Utility {
    class CollisionDetection {
        public static void resourceCollisionDetection() {
            // TODO DEBUG - Starts collision Timer
            if(GameController.Resources != null) {
                FrameDebugTimer.startCollisionTimer();
                List<IResource> toRemove = new List<IResource>();
                foreach(IResource resource in GameController.Resources) {
                    if(resource.Shape.Polygon.RenderedGeometry.Bounds.IntersectsWith(GameController.Player.PlayerShip.Shape.Polygon.RenderedGeometry.Bounds) &&
                            !resource.Shape.Polygon.Visibility.Equals(Visibility.Collapsed)) {
                        GameController.Player.Wallet += resource.Value;
                        toRemove.Add(resource);
                        //resource.Polygon.Visibility = Visibility.Hidden;
                    }
                }
                foreach(IResource resource in toRemove) {
                    GameController.Resources.Remove(resource);
                    resource.Shape.Polygon.Visibility = Visibility.Collapsed;
                }
                toRemove.Clear();
                // TODO DEBUG - Stops collision Timer
                FrameDebugTimer.stopCollisionTimer(); 
            }
        }
    }
}
