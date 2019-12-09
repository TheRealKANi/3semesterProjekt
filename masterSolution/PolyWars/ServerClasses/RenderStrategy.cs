using PolyWars.API.Model.Interfaces;
using PolyWars.API.Strategies;
using PolyWars.Logic;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PolyWars.ServerClasses {
    class RenderStrategy : IRenderStrategy {
        public Polygon Render(IRenderable r, IRay ray) {
            double verticeAngle = 360d / r.Vertices;
            Polygon p;
            return UIDispatcher.Invoke(() => {
                p = new Polygon() {
                    Stroke = new SolidColorBrush(r.BorderColor),
                    Fill = new SolidColorBrush(r.FillColor),
                    StrokeThickness = r.StrokeThickness
                };

                List<Point> pc = new List<Point>();

                for(int i = 0; i < r.Vertices; i++) {
                    double radians = Math.PI / 2 - ((verticeAngle * i + ray.Angle) / 180) * Math.PI;
                    pc.Add(new Point() {
                        X = ray.CenterPoint.X + Math.Cos(radians) * r.Width / 2,
                        Y = ray.CenterPoint.Y + Math.Sin(radians) * r.Height / 2
                    });
                }

                foreach(Point point in pc) {
                    p.Points.Add(point);
                }
                return p;
            });
        }
    }
    class RenderWithHeaderStrategy : RenderStrategy, IRenderStrategy {
        public new Polygon Render(IRenderable r, IRay ray) {
            return UIDispatcher.Invoke(() => {
                Polygon p = base.Render(r, ray);
                PointCollection pc = p.Points;
                pc.Add(pc[0]);
                pc.Add(ray.CenterPoint);
                pc.Add(pc[0]);
                p.Points = pc;

                return p;
            });
        }
    }
}
