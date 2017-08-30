using System;
using System.Collections.Generic;
using System.Threading;

namespace SineGraph {
    internal class SineGraph {
        struct Line {
            public bool Background;
            public double CurAngle;
            public ushort LastCurDraw;
            public ushort CurDraw;
        }
        private const ushort MaxDraw = 129;
        private double Sharpness { get; set; }
        private Line[] lines = new Line[5];

        public SineGraph() {
            Sharpness = 0.033;
            for (int j = 0; j < lines.Length; j++) {
                lines[j].CurAngle = 1.2*j * Math.PI / (lines.Length);
                //lines[j].Background = j % 2 == 0 ? j > lines.Length / 2 - 1 : j > lines.Length / 2;
            }

        }

        private void CalculateCurDraw() {
            for (int j = 0; j < lines.Length; j++) {
                lines[j].LastCurDraw = lines[j].CurDraw;
                lines[j].CurDraw =
                    Convert.ToUInt16(Convert.ToUInt16(MaxDraw / 2) + Math.Sin(lines[j].CurAngle) * MaxDraw / 2);
                lines[j].CurAngle += Sharpness;
                //if ((lines[j].CurDraw - lines[j].lastCurDraw > 0) || (lines[j].CurDraw == 1 && lines[j].lastCurDraw == 0))
                    lines[j].Background = lines[j].CurDraw - lines[j].LastCurDraw > 0;
            }
        }

        private void DrawAsteriks() {
            ushort i = 0;
            for (; i < MaxDraw; i++)
                for (int j = 0; j < lines.Length; j++)
                    if (i == lines[j].CurDraw)
                        Console.Write(lines[j].Background ? '\u2B1B' : '\u25AA');
                else if(j == lines.Length-1)
                    Console.Write(' ');
            Console.WriteLine();  
        }

        private void LCalculateCurDraw() {
            while (true) {
                CalculateCurDraw();
                Thread.Sleep(16);
            }
        }

        private void LDrawAsteriks() {
            while (true) {
                DrawAsteriks();
                Thread.Sleep(16);
            }
        }

        public void Draw() {
            Thread calculations = new Thread(LCalculateCurDraw);
            Thread drawing = new Thread(LDrawAsteriks);
            
            calculations.Start();
            drawing.Start();
        }
        
    }
    
    
    internal class Program {
        public static void Main(string[] args) {
            SineGraph test = new SineGraph();
            

            test.Draw();
        }
    }
}