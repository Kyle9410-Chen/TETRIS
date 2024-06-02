using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TETRIS
{
    class block
    {
        public Point[] blockPoint = new Point[4] { new Point(), new Point(), new Point(), new Point() };
        public string blocktype;
        public int blockstyle;

        public void setPoint(string type, int style)
        {
            blocktype = type;
            blockstyle = style;
            switch (type)
            {
                case "I":
                    switch (style)
                    {
                        case 1:
                            blockPoint[0] = new Point(-1, 0);
                            blockPoint[1] = new Point(0, 0);
                            blockPoint[2] = new Point(1, 0);
                            blockPoint[3] = new Point(2, 0);
                            break;
                        case 2:
                            blockPoint[0] = new Point(1, -1);
                            blockPoint[1] = new Point(1, 0);
                            blockPoint[2] = new Point(1, 1);
                            blockPoint[3] = new Point(1, 2);
                            break;
                        case 3:
                            blockPoint[0] = new Point(-1, 1);
                            blockPoint[1] = new Point(0, 1);
                            blockPoint[2] = new Point(1, 1);
                            blockPoint[3] = new Point(2, 1);
                            break;
                        case 4:
                            blockPoint[0] = new Point(0, -1);
                            blockPoint[1] = new Point(0, 0);
                            blockPoint[2] = new Point(0, 1);
                            blockPoint[3] = new Point(0, 2);
                            break;
                    }
                    break;
                case "J":
                    switch (style)
                    {
                        case 1:
                            blockPoint[0] = new Point(-1, -1);
                            blockPoint[1] = new Point(-1, 0);
                            blockPoint[2] = new Point(0, 0);
                            blockPoint[3] = new Point(1, 0);
                            break;
                        case 2:
                            blockPoint[0] = new Point(1, -1);
                            blockPoint[1] = new Point(0, -1);
                            blockPoint[2] = new Point(0, 0);
                            blockPoint[3] = new Point(0, 1);
                            break;
                        case 3:
                            blockPoint[0] = new Point(1, 1);
                            blockPoint[1] = new Point(-1, 0);
                            blockPoint[2] = new Point(0, 0);
                            blockPoint[3] = new Point(1, 0);
                            break;
                        case 4:
                            blockPoint[0] = new Point(-1, 1);
                            blockPoint[1] = new Point(0, -1);
                            blockPoint[2] = new Point(0, 0);
                            blockPoint[3] = new Point(0, 1);
                            break;
                    }
                    break;
                case "L":
                    switch (style)
                    {
                        case 1:
                            blockPoint[0] = new Point(1, -1);
                            blockPoint[1] = new Point(-1, 0);
                            blockPoint[2] = new Point(0, 0);
                            blockPoint[3] = new Point(1, 0);
                            break;
                        case 2:
                            blockPoint[0] = new Point(1, 1);
                            blockPoint[1] = new Point(0, -1);
                            blockPoint[2] = new Point(0, 0);
                            blockPoint[3] = new Point(0, 1);
                            break;
                        case 3:
                            blockPoint[0] = new Point(-1, 1);
                            blockPoint[1] = new Point(-1, 0);
                            blockPoint[2] = new Point(0, 0);
                            blockPoint[3] = new Point(1, 0);
                            break;
                        case 4:
                            blockPoint[0] = new Point(-1, -1);
                            blockPoint[1] = new Point(0, -1);
                            blockPoint[2] = new Point(0, 0);
                            blockPoint[3] = new Point(0, 1);
                            break;
                    }
                    break;
                case "O":
                    switch (style)
                    {
                        case 1:
                            blockPoint[0] = new Point(0, -1);
                            blockPoint[1] = new Point(0, 0);
                            blockPoint[2] = new Point(1, -1);
                            blockPoint[3] = new Point(1, 0);
                            break;
                        case 2:
                            blockPoint[0] = new Point(0, -1);
                            blockPoint[1] = new Point(0, 0);
                            blockPoint[2] = new Point(1, -1);
                            blockPoint[3] = new Point(1, 0);
                            break;
                        case 3:
                            blockPoint[0] = new Point(0, -1);
                            blockPoint[1] = new Point(0, 0);
                            blockPoint[2] = new Point(1, -1);
                            blockPoint[3] = new Point(1, 0);
                            break;
                        case 4:
                            blockPoint[0] = new Point(0, -1);
                            blockPoint[1] = new Point(0, 0);
                            blockPoint[2] = new Point(1, -1);
                            blockPoint[3] = new Point(1, 0);
                            break;
                    }
                    break;
                case "S":
                    switch (style)
                    {
                        case 1:
                            blockPoint[0] = new Point(-1, 0);
                            blockPoint[1] = new Point(0, 0);
                            blockPoint[2] = new Point(0, -1);
                            blockPoint[3] = new Point(1, -1);
                            break;
                        case 2:
                            blockPoint[0] = new Point(0, -1);
                            blockPoint[1] = new Point(0, 0);
                            blockPoint[2] = new Point(1, 0);
                            blockPoint[3] = new Point(1, 1);
                            break;
                        case 3:
                            blockPoint[0] = new Point(-1, 1);
                            blockPoint[1] = new Point(0, 1);
                            blockPoint[2] = new Point(0, 0);
                            blockPoint[3] = new Point(1, 0);
                            break;
                        case 4:
                            blockPoint[0] = new Point(-1, -1);
                            blockPoint[1] = new Point(-1, 0);
                            blockPoint[2] = new Point(0, 0);
                            blockPoint[3] = new Point(0, 1);
                            break;
                    }
                    break;
                case "T":
                    switch (style)
                    {
                        case 1:
                            blockPoint[0] = new Point(-1, 0);
                            blockPoint[1] = new Point(0, 0);
                            blockPoint[2] = new Point(1, 0);
                            blockPoint[3] = new Point(0, -1);
                            break;
                        case 2:
                            blockPoint[0] = new Point(0, -1);
                            blockPoint[1] = new Point(0, 0);
                            blockPoint[2] = new Point(0, 1);
                            blockPoint[3] = new Point(1, 0);
                            break;
                        case 3:
                            blockPoint[0] = new Point(-1, 0);
                            blockPoint[1] = new Point(0, 0);
                            blockPoint[2] = new Point(1, 0);
                            blockPoint[3] = new Point(0, 1);
                            break;
                        case 4:
                            blockPoint[0] = new Point(-1, 0);
                            blockPoint[1] = new Point(0, -1);
                            blockPoint[2] = new Point(0, 0);
                            blockPoint[3] = new Point(0, 1);
                            break;
                    }
                    break;
                case "Z":
                    switch (style)
                    {
                        case 1:
                            blockPoint[0] = new Point(-1, -1);
                            blockPoint[1] = new Point(0, -1);
                            blockPoint[2] = new Point(0, 0);
                            blockPoint[3] = new Point(1, 0);
                            break;
                        case 2:
                            blockPoint[0] = new Point(1, -1);
                            blockPoint[1] = new Point(1, 0);
                            blockPoint[2] = new Point(0, 0);
                            blockPoint[3] = new Point(0, 1);
                            break;
                        case 3:
                            blockPoint[0] = new Point(-1, 0);
                            blockPoint[1] = new Point(0, 0);
                            blockPoint[2] = new Point(0, 1);
                            blockPoint[3] = new Point(1, 1);
                            break;
                        case 4:
                            blockPoint[0] = new Point(0, -1);
                            blockPoint[1] = new Point(0, 0);
                            blockPoint[2] = new Point(-1, 0);
                            blockPoint[3] = new Point(-1, 1);
                            break;
                    }
                    break;
            }
        }
    }
}
