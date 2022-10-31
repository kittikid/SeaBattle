using SeaBattleMainWindow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using static SeaBattleMainWindow.Enums;

namespace SeaBattle
{
    public class SeaBattleMainWindow : Control
    {
        public SeaBattleMainWindow() : base()
        {
            _cellSize = 30;
            _cellPadding = 3;
            _ShipColor.Add(Color.White);
            _ShipColor.Add(Color.Red);
            _ShipColor.Add(Color.Green);
            _ShipColor.Add(Color.Blue);
            _ShipColor.Add(Color.Yellow);
            //если мимо
            _ShipColor.Add(Color.LightBlue);
            //если попал
            _ShipColor.Add(Color.Gray);
            //если убил
            _ShipColor.Add(Color.Black);
        }

        protected EnumCellColor[,] _playerMap = new EnumCellColor[10, 10];
        protected EnumCellColor[,] _botMap = new EnumCellColor[10, 10];

        protected int[,] _ship = new int[10, 10];

        protected char[] _aplhabet = new char[] { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'К' };
        protected List<Color> _ShipColor = new List<Color>();
        protected int[] _ColorSet = new int[] { 1, 2, 3, 4 };

        protected List<int> playerlifeShips = new List<int> { 4, 6, 6, 4 };

        protected int _cellSize;
        protected int _cellPadding;

        protected bool _isPlaying = false;
        protected bool _orientation = false;

        protected Random _random = new Random();

        public bool Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }
        public bool isPlaying
        {
            get { return _isPlaying; }
            set { if (_isPlaying != value) _isPlaying = value; }
        }
        private int CellSize
        {
            get { return _cellSize; }
            set
            {
                if (value != _cellSize)
                {
                    _cellSize = value;
                    Invalidate();
                }
            }
        }
        private int CellPadding
        {
            get { return _cellPadding; }
            set
            {
                if (value != _cellPadding)
                {
                    _cellPadding = value;
                    Invalidate();
                }
            }
        }

        //объект собственного класса
        Ship currentShip = new Ship(0, 0, 4, true);

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        
        //художник
        protected override void OnPaint(PaintEventArgs e)
        {
            //отрисовка фона
            Rectangle backgroung = new Rectangle(0, 0, Width, Height);
            e.Graphics.FillRectangle(new SolidBrush(Color.LightBlue), backgroung);

            //отрисовка поля игрока
            DrawMap(e, _playerMap);

            //отрисовка поля бота
            DrawMap(e, _botMap);

            //Rectangle cellRectangle;
            //Brush cellColor = new SolidBrush(Color.White);
            //int cellSize = CellSize + CellPadding;
            ////text
            //int fontSize = CellSize / 3;
            //Font font = new Font("Segoe Script", fontSize);
            //StringFormat sf = new StringFormat();
            //sf.Alignment = StringAlignment.Center;
            //sf.LineAlignment = StringAlignment.Center;

            ////поле игрока
            ////для всех последующих ячеек пользователя
            //for (int i = 0; i < _playerMap.GetLength(0); i++)
            //{
            //    for (int j = 0; j < _playerMap.GetLength(1); j++)
            //    {
            //        cellRectangle = new Rectangle(cellSize * (i + 1), cellSize * (j + 1), _cellSize, _cellSize);
            //        e.Graphics.FillRectangle(new SolidBrush(_ShipColor[_playerMap[i, j]]), cellRectangle);
            //    }
            //    //number
            //    cellRectangle = new Rectangle(_cellPadding, cellSize * (i + 1), _cellSize, _cellSize);
            //    e.Graphics.DrawString((i + 1).ToString(), font, new SolidBrush(Color.Black), cellRectangle, sf);
            //    //alphabet
            //    cellRectangle = new Rectangle(cellSize * (i + 1), _cellPadding, _cellSize, _cellSize);
            //    e.Graphics.DrawString(_aplhabet[i].ToString(), font, new SolidBrush(Color.Black), cellRectangle, sf);
            //}

            ////поле бота
            ////для всех последующих ячеек бота
            //for (int i = 0; i < _botMap.GetLength(0); i++)
            //{
            //    for (int j = 0; j < _botMap.GetLength(1); j++)
            //    {
            //        cellRectangle = new Rectangle(cellSize * (i + _botMap.GetLength(0) + 3), cellSize * (j + 1), _cellSize, _cellSize);
            //        e.Graphics.FillRectangle(new SolidBrush(_ShipColor[_botMap[j, i]]), cellRectangle);
            //    }
            //    //number
            //    cellRectangle = new Rectangle(cellSize * (_botMap.GetLength(0) + 2), cellSize * (i + 1), _cellSize, _cellSize);
            //    e.Graphics.DrawString((i + 1).ToString(), font, new SolidBrush(Color.Black), cellRectangle, sf);
            //    //alphabet
            //    cellRectangle = new Rectangle(cellSize * (i + _botMap.GetLength(0) + 3), _cellPadding, _cellSize, _cellSize);
            //    e.Graphics.DrawString(_aplhabet[i].ToString(), font, new SolidBrush(Color.Black), cellRectangle, sf);
            //}

            //отрисовка текущих кораблей
            int x, y;

            for (int i = 0; i < _ship.GetLength(0); i++)
                for (int j = 0; j < _ship.GetLength(1); j++)
                    if (_ship[i, j] != 0)
                    {
                        x = _cellSize * (i + 1) + _cellPadding;
                        y = _cellSize * (j + _playerMap.GetLength(1) + 3) + _cellPadding;
                        e.Graphics.FillRectangle(new SolidBrush(Color.Blue), x, y, _cellSize, _cellSize);
                    }
        }

        //отрисовка полей игрока и бота 
        private void DrawMap(PaintEventArgs e, EnumCellColor[,] map)
        {
            Rectangle cellRectangle;
            Brush cellColor = new SolidBrush(Color.White);
            int cellSize = CellSize + CellPadding;
            //text
            int fontSize = CellSize / 3;
            Font font = new Font("Segoe Script", fontSize);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            string bufVarMap;
            string mapI, mapJ; 

            //красота
            if (map == _botMap)
                bufVarMap = $"{map.GetLength(0) + 3}";
            else
                bufVarMap = $"{1}";

            //для всех последующих ячеек
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    //помощник рисования карты
                    if (map == _botMap) { 
                        mapI = $"{j}"; mapJ = $"{i}";
                    }
                    else {
                        mapI = $"{i}"; mapJ = $"{j}";
                    }

                    cellRectangle = new Rectangle(cellSize * (i + int.Parse(bufVarMap)), cellSize * (j + 1), _cellSize, _cellSize);
                    e.Graphics.FillRectangle(new SolidBrush(_ShipColor[(int)map[int.Parse(mapI), int.Parse(mapJ)]]), cellRectangle);
                }
                //циферки
                if (map == _botMap)
                    cellRectangle = new Rectangle(cellSize * (map.GetLength(0) + 2), cellSize * (i + 1), _cellSize, _cellSize);
                else
                    cellRectangle = new Rectangle(_cellPadding, cellSize * (i + 1), _cellSize, _cellSize);
                e.Graphics.DrawString((i + 1).ToString(), font, new SolidBrush(Color.Black), cellRectangle, sf);
                //буковки
                cellRectangle = new Rectangle(cellSize * (i + int.Parse(bufVarMap)), _cellPadding, _cellSize, _cellSize);
                e.Graphics.DrawString(_aplhabet[i].ToString(), font, new SolidBrush(Color.Black), cellRectangle, sf);
            }
        }

        //расстановка кораблей
        //public int[,] ConfigureShips2(bool valueWhoseMap)
        //{
        //    int[,] map;

        //    int lengthShip = 4;
        //    int cycleValue = 4;
        //    int shipsCount = 10;

        //    if (valueWhoseMap) map = _playerMap;
        //    else map = _botMap;

        //    ClearMap(map);

        //    while (shipsCount > 0)
        //    {
        //        if (_random.Next(1, 101) % 2 == 0)
        //            _orientation = true;
        //        else
        //            _orientation = false;

        //        for (int i = 0; i < cycleValue / 4; ++i)
        //        {
        //            int posX = _random.Next(0, 10);
        //            int posY = _random.Next(0, 10);

        //            if (_orientation)
        //            {
        //                while (!IsInsideMap(posX, posY + lengthShip - 1) || !IsEmpty(posX, posY, lengthShip, _orientation, map))
        //                {
        //                    posX = _random.Next(0, 10);
        //                    posY = _random.Next(0, 10);
        //                }

        //                for (int k = posY; k < posY + lengthShip; ++k)
        //                {
        //                    if (map[posX, k] == 0)
        //                        map[posX, k] = lengthShip;
        //                }
        //            }
        //            else
        //            {
        //                while (!IsInsideMap(posX + lengthShip - 1, posY) || !IsEmpty(posX, posY, lengthShip, _orientation, map))
        //                {
        //                    posX = _random.Next(0, 10);
        //                    posY = _random.Next(0, 10);
        //                }

        //                for (int k = posX; k < posX + lengthShip; ++k)
        //                {
        //                    if (map[k, posY] == 0)
        //                        map[k, posY] = lengthShip;
        //                }
        //            }

        //            NerbaryShips(posX, posY, lengthShip, _orientation, map);

        //            --shipsCount;
        //            if (shipsCount <= 0)
        //                break;
        //        }
        //        cycleValue += 4;
        //        --lengthShip;
        //    }

        //    Normalized(map);
        //    Invalidate();
        //    return map;
        //}

        public EnumCellColor[,] ConfigureShips(bool valueWhoseMap)
        {
            EnumCellColor[,] map;

            currentShip.lengthShip = 4;
            currentShip.countShip();
            int shipsCount = 10;
            
            //выборвка координат
            int begin = 0; 
            int end = 10;

            if (valueWhoseMap) map = _playerMap;
            else map = _botMap;

            ClearMap(map);

            while (shipsCount > 0)
            {
                if (_random.Next(1, 101) % 2 == 0)
                    _orientation = true;
                else
                    _orientation = false;

                for (int i = 0; i < currentShip.count; ++i)
                {
                    currentShip.x = _random.Next(begin, end);
                    currentShip.y = _random.Next(begin, end);

                    if (_orientation)
                    {
                        while (!IsInsideMap(currentShip.x, currentShip.y + currentShip.lengthShip - 1) || !IsEmpty(currentShip.x, currentShip.y, currentShip.lengthShip, _orientation, map))
                        {
                            currentShip.x = _random.Next(begin, end);
                            currentShip.y = _random.Next(begin, end);
                        }

                        for (int k = currentShip.y; k < currentShip.y + currentShip.lengthShip; ++k)
                        {
                            if (map[currentShip.x, k] == 0)
                                map[currentShip.x, k] = (EnumCellColor)currentShip.lengthShip;
                        }
                    }
                    else
                    {
                        while (!IsInsideMap(currentShip.x + currentShip.lengthShip - 1, currentShip.y) || !IsEmpty(currentShip.x, currentShip.y, currentShip.lengthShip, _orientation, map))
                        {
                            currentShip.x = _random.Next(begin, end);
                            currentShip.y = _random.Next(begin, end);
                        }

                        for (int k = currentShip.x; k < currentShip.x + currentShip.lengthShip; ++k)
                        {
                            if (map[k, currentShip.y] == 0)
                                map[k, currentShip.y] = (EnumCellColor)currentShip.lengthShip;
                        }
                    }

                    NerbaryShips(currentShip.x, currentShip.y, currentShip.lengthShip, _orientation, map);

                    --shipsCount;
                    if (shipsCount <= 0)
                        break;
                }

                ++currentShip.count;
                --currentShip.lengthShip;
            }

            Normalized(map);
            Invalidate();
            return map;
        }
        
        //for (int i = 0; i<currentShip.lifeShips.Count; ++i)
        //            currentShip.lifeShips[i] = i == 0 || i == 3 ? 4 : 6;

        private enum EnumDir { Up = 0, Left, Down, Right }

        //робот стрелять
        protected bool BotShoot(EnumCellColor[,] map, int x = -1, int y = -1, int bufX = 0, int bufY = 0)
        {
            bool hit;
            Random random = new Random();

            if (x == -1 && y == -1)
            {
                x = random.Next(0, 10);
                y = random.Next(0, 10);
            }
            else
            {
                //int whereShoot = random.Next(0, 4);
                // 0 - вверх, 1 - влево, 2 - вниз, 3 - вправо

                EnumDir dir = (EnumDir)random.Next(0, 4);

                if (dir == EnumDir.Up) y += y == 0 ? 1 : -1;
                else if (dir == EnumDir.Left) x += x == 0 ? 1 : -1;
                else if (dir == EnumDir.Down) y -= y == 9 ? 1 : -1;
                else if (dir == EnumDir.Right) x -= x == 9 ? 1 : -1;
            }

            while (map[x, y] == EnumCellColor.BackColor || map[x, y] == EnumCellColor.WoundedShip || map[x, y] == EnumCellColor.SlainShip)
            {
                if (map[bufX, bufY] != EnumCellColor.WoundedShip)
                {
                    x = random.Next(0, 10);
                    y = random.Next(0, 10);
                } 
                else
                {
                    switch (random.Next(0, 4))
                    {
                        case 0:
                            if (y < 1)
                                goto case 2;
                            --y;
                            break;
                        case 1:
                            if (x < 1)
                                goto case 3;
                            --x;
                            break;
                        case 2:
                            if (y > 8)
                                goto case 0;
                            ++y;
                            break;
                        case 3:
                            if (x > 8)
                                goto case 1;
                            ++x;
                            break;
                    }
                }
            }

            if (map[x, y] > EnumCellColor.UntouchedCell && map[x, y] < EnumCellColor.BackColor)
            {
                hit = true;

                bufX = x;
                bufY = y;

                --playerlifeShips[4 - (int)map[x, y]];
                map[x, y] = (!NearCells(map, x, y) && playerlifeShips[4 - (int)map[x, y]] % (int)map[x, y] == 0) ? EnumCellColor.SlainShip : EnumCellColor.WoundedShip;
            }
            else
            {
                if (map[bufX, bufY] == EnumCellColor.WoundedShip)
                {
                    x = bufX;
                    y = bufY;
                    BotShoot(map, x, y);
                }
                hit = false;
                map[x, y] = EnumCellColor.BackColor;
            }

            if (hit)
                BotShoot(map, x, y, bufX, bufY);
            

            return hit;
        }

        //очищение поля вокруг убитого корабля(пусто)
        protected void AroundClear(EnumCellColor[,] map, int x, int y)
        {

        }

        //стрельба игрока
        public void Shoot(object sender, MouseEventArgs e)
        {
            if (isPlaying)
            {
                int X = e.X;
                int Y = e.Y;
                bool playerTurn = PlayerShoot(_botMap, X, Y);
                if (!playerTurn)
                    BotShoot(_playerMap);

                if (!CheckIfMapIsNotEmpty(_playerMap))
                {
                    MessageBox.Show(
                        "Вы проиграли",
                        "Поражение",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly
                        );
                }

                if (!CheckIfMapIsNotEmpty(_botMap))
                {
                    MessageBox.Show(
                        "Вы победили",
                        "Победа",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly
                        );
                }

                if (!CheckIfMapIsNotEmpty(_playerMap) || !CheckIfMapIsNotEmpty(_botMap))
                {
                    ClearMap(_botMap);
                    ClearMap(_playerMap);
                    isPlaying = false;
                }
            }
            Invalidate();
        }

        //логика стрельбы игрока (norm)
        protected bool PlayerShoot(EnumCellColor[,] map, int X, int Y)
        {
            if (!isPlaying) return false;

            bool hit = false;

            int x = (Y - 30) / (_cellSize + _cellPadding); 
            int y = (X - 430) / (_cellSize + _cellPadding);

            if (!IsInsideMap(x, y)) return false; 
            else hit = true;
            
            
            if (map[x, y] > 0 && map[x, y] < EnumCellColor.BackColor)
            {
                hit = true;

                --currentShip.lifeShips[4 - (int)map[x, y]];
                map[x, y] = (!NearCells(map, x, y) && currentShip.lifeShips[4 - (int)map[x, y]] % (int)map[x, y] == 0) ? EnumCellColor.SlainShip : EnumCellColor.WoundedShip;
            }
            else
            {
                if (map[x, y] != EnumCellColor.BackColor && map[x, y] != EnumCellColor.WoundedShip && map[x, y] != EnumCellColor.SlainShip)
                {
                    hit = false;
                    map[x, y] = EnumCellColor.BackColor;
                }
                else hit = true;
            }
            
            Invalidate();
            
            return hit;
        }

        //проверка на цвета рядом стрельба
        protected bool NearCells(EnumCellColor[,] map, int x, int y)
        {
            bool flag = false;

            foreach (int i in _ColorSet)
            {
                flag =
                    (IsInsideMap(x + 1, y) && i == (int)map[x + 1, y]) ||
                    (IsInsideMap(x - 1, y) && i == (int)map[x - 1, y]) ||
                    (IsInsideMap(x, y + 1) && i == (int)map[x, y + 1]) ||
                    (IsInsideMap(x, y - 1) && i == (int)map[x, y - 1]);
            }


            return flag;
        }

        //поле для кораблей
        public void FieldForShips()
        {
            //убрать все корабли с поля плеера
            ClearMap(_playerMap);

            currentShip.x = 0;
            currentShip.y = 0;
            currentShip.lengthShip = 4;
            currentShip.orientation = true;
            currentShip.countShip();

            CreateCurrentShipPlayer(currentShip.x, currentShip.lengthShip);
        }

        private bool PlaceShip = false;
        //реализация расставления кораблей в ручную
        public void PlacementShipsPlayer(Point MousePos)
        {
            Point MousePos1 = PointToClient(MousePos);
            int _xCor = MousePos1.Y / (_cellSize + _cellPadding); // норм
            int _yCor = (MousePos1.X - 430) / (_cellSize + _cellPadding); // норм

            //MessageBox.Show("x: " + _xCor.ToString() + " " + "y: " + _yCor.ToString());

            if (PlaceShip)
            {

                //if (IsEmpty(_xCor, _yCor, lengthShip, _orientation, _playerMap) && IsInsideMap(_xCor, _yCor))
                //{
                _xCor = MousePos1.Y / (_cellSize + _cellPadding) - 1;
                _yCor = (MousePos1.X) / (_cellSize + _cellPadding) - 1;

                //MessageBox.Show("x: " + _xCor.ToString() + " " + "y: " + _yCor.ToString());

                if (IsInsideMap(_xCor, _yCor))
                {
                    currentShip.orientation = _orientation;
                    int valueShip = currentShip.lengthShip;

                    //NerbaryShips(_xCor, _yCor, lengthShip, _orientation, _playerMap);

                    OrientationShip(_yCor, _xCor, valueShip, currentShip.orientation, _playerMap);

                    //вычесть из списка кол-во кораблей
                    currentShip.countShips[0] -= 1;
                    if (currentShip.countShips.Count > 0 && currentShip.countShips[0] == 0)
                    {
                        currentShip.lengthShip -= 1;
                        currentShip.countShips.RemoveAt(0);
                    }

                    //автозаполнение корабля поля
                    for (int i = 0; i < 9; i++)
                        for (int j = 0; j < 9; j++)
                            _ship[i, j] = 0;

                    for (int i = 0; i <= currentShip.x; i++)
                        for (int j = 0; j < currentShip.lengthShip; j++)
                            _ship[i, j] = currentShip.lengthShip;

                    if (currentShip.countShips.Count == 0) PlaceShip = false;

                    Invalidate();
                    //}
                }
            }
            else
            {
                //попали на корабль
                if (MousePos1.Y >= 390 && MousePos1.Y < 510
                    && MousePos1.X >= 33 && MousePos1.X <= 63)
                {
                    MessageBox.Show("Проставьте корабли подряд", "Расстановка началась");
                    if (PlaceShip) PlaceShip = false;
                    else PlaceShip = true;
                }

                //Shoot(_playerMap, _xCor, _yCor);
                //testDraw(_xCor, _yCor, MousePos1);
            }
        }

        //изменение ориентации по нажатию клавиши
        public void KeyOrientation(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z)
                _orientation = !_orientation;
        }

        //public void DragShip(DragEventArgs e)
        //{
        //    e.Effect = DragDropEffects.Move;
        //}

        //проверка ориентации
        private bool OrientationShip(int i, int j, int valueShip, bool place, EnumCellColor[,] cell)
        {

            if (IsInsideMap(i, j))
            {
                if (place)
                {
                    for (int count = valueShip; count != 0; ++j)
                {
                    cell[i, j] = (EnumCellColor)valueShip;
                    --count;
                }
                return true;
                }
                else
                {
                    for (int count = valueShip; count != 0; ++i)
                    {
                        cell[i, j] = (EnumCellColor)valueShip;
                        --count;
                    }
                    return true;
                }
            } 
            return false;
        }

        //проверка поля на наличие кораблей
        protected bool CheckIfMapIsNotEmpty(EnumCellColor[,] map)
        {
            if (isPlaying)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (map[i, j] > EnumCellColor.UntouchedCell && map[i, j] < EnumCellColor.BackColor)
                        {
                            return false;
                        }
                    }
                }   
                return true;
            }
            else return true;
        }

        //расстановка кораблей -1 вокруг
        protected void NerbaryShips(int posX, int posY, int lengthShip, bool orientation, EnumCellColor[,] map)
        {
            if (_orientation)
            {
                for (int k = posY; k < posY + lengthShip; ++k)
                {
                    if (k - 1 >= 0 && map[posX, k - 1] == 0)    
                        map[posX, k - 1] = EnumCellColor.UntouchedCell - 1;

                    if (k + 1 < 10 && map[posX, k + 1] == 0)
                        map[posX, k + 1] = EnumCellColor.UntouchedCell - 1;

                    if (posX - 1 >= 0 && map[posX - 1, k] == 0)
                        map[posX - 1, k] = EnumCellColor.UntouchedCell - 1;

                    if (posX + 1 < 10 && map[posX + 1, k] == 0)
                        map[posX + 1, k] = EnumCellColor.UntouchedCell - 1;

                    if (posX - 1 >= 0 && k - 1 >= 0 && map[posX - 1, k - 1] == 0)
                        map[posX - 1, k - 1] = EnumCellColor.UntouchedCell - 1;

                    if (posX + 1 < 10 && k - 1 >= 0 && map[posX + 1, k - 1] == 0)
                        map[posX + 1, k - 1] = EnumCellColor.UntouchedCell - 1;

                    if (posX - 1 >= 0 && k + 1 < 10 && map[posX - 1, k + 1] == 0)
                        map[posX - 1, k + 1] = EnumCellColor.UntouchedCell - 1;

                    if (posX + 1 < 10 && k + 1 < 10 && map[posX + 1, k + 1] == 0)
                        map[posX + 1, k + 1] = EnumCellColor.UntouchedCell - 1;
                }
            }
            else
            {
                for (int k = posX; k < posX + lengthShip; ++k)
                {
                    if (posY - 1 >= 0 && map[k, posY - 1] == 0)
                        map[k, posY - 1] = EnumCellColor.UntouchedCell - 1;

                    if (posY + 1 < 10 && map[k, posY + 1] == 0)
                        map[k, posY + 1] = EnumCellColor.UntouchedCell - 1;

                    if (k - 1 >= 0 && map[k - 1, posY] == 0)
                        map[k - 1, posY] = EnumCellColor.UntouchedCell - 1;

                    if (k + 1 < 10 && map[k + 1, posY] == 0)
                        map[k + 1, posY] = EnumCellColor.UntouchedCell - 1;

                    if (k - 1 >= 0 && posY - 1 >= 0 && map[k - 1, posY - 1] == 0)
                        map[k - 1, posY - 1] = EnumCellColor.UntouchedCell - 1;

                    if (k + 1 < 10 && posY - 1 >= 0 && map[k + 1, posY - 1] == 0)
                        map[k + 1, posY - 1] = EnumCellColor.UntouchedCell - 1;

                    if (k - 1 >= 0 && posY + 1 < 10 && map[k - 1, posY + 1] == 0)
                        map[k - 1, posY + 1] = EnumCellColor.UntouchedCell - 1;

                    if (k + 1 < 10 && posY + 1 < 10 && map[k + 1, posY + 1] == 0)
                        map[k + 1, posY + 1] = EnumCellColor.UntouchedCell - 1;
                }
            }
        }

        //нормалихация матрицы после расстановки кораблей
        protected void Normalized(EnumCellColor[,] map)
        {
            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    if (map[i, j] < EnumCellColor.UntouchedCell)
                        map[i, j] = EnumCellColor.UntouchedCell;
                }
            }
        }

        //проверка на свободное место
        protected bool IsEmpty(int i, int j, int length, bool orientation, EnumCellColor[,] map)
        {
            //попытка избавиться от вложенности
            //int buf;
            //int x1 = 0, x2 = 0; 

            //if (_orientation)
            //{
            //    buf = j;
            //    x1 = i;

            //}
            //else
            //{
            //    buf = i;
            //    x2 = j;
            //}

            //for (int k = buf; k < buf + length; ++k)
            //{
            //    x1 = x1 == 0 ? k : x1;
            //    x2 = x2 == 0 ? k : x2;
            //    if (map[x1, x2] != 0)
            //        return false;
            //}

            if (_orientation)
            {
                for (int k = j; k < j + length; ++k)
                    if (map[i, k] != 0)
                        return false;
            }
            else
            {
                for (int k = i; k < i + length; ++k)
                    if (map[k, j] != 0)
                        return false;
            }

            return true;
        }

        //очищение матрицы
        protected void ClearMap(EnumCellColor[,] map)
        {
            for (int i = 0; i < map.GetLength(0); ++i)
                for (int j = 0; j < map.GetLength(1); ++j)
                    map[i, j] = EnumCellColor.UntouchedCell;
        }

        //ограничение поля стрельбы
        protected bool IsInsideMap(int i, int j)
        {
            if (i < 0 || j < 0 || i >= 10 || j >= 10)
                return false;
            return true;
        }

        //расставить снизу поле
        public void CreateCurrentShipPlayer(int x, int typeShip)
        {
            for (int i = 0; i <= x; i++)
                for (int j = 0; j < typeShip; j++)
                    _ship[i, j] = typeShip;

            Invalidate();
        }
    }
}
