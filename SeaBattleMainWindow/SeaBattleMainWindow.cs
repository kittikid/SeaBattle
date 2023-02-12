using SeaBattleMainWindow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
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
            //клетка не тронута
            _ShipColor.Add(Color.White);
            //кораблики
            _ShipColor.Add(Color.Red);
            _ShipColor.Add(Color.Green);
            _ShipColor.Add(Color.Blue);
            _ShipColor.Add(Color.Yellow);   
            //если мимо
            _ShipColor.Add(UserBackColor);
            //если попал
            _ShipColor.Add(Color.Gray);
            //если убил
            _ShipColor.Add(Color.Black);
            //настройка текста
            UserFontColor = Color.Black;
            UserFontFont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);
            EventAroundClear += AroundClear;
        }

        protected EnumCellColor[,] _playerMap = new EnumCellColor[10, 10];
        protected EnumCellColor[,] _botMap = new EnumCellColor[10, 10];
        protected EnumCellColor[,] _botBattleAIMap = new EnumCellColor[10, 10];

        protected int[,] _ship = new int[10, 10];

        protected char[] _aplhabet = new char[] { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'К' };
        protected List<Color> _ShipColor = new List<Color>();
        protected int[] _ColorSet = new int[] { 1, 2, 3, 4 };

        protected List<int> playerlifeShips = new List<int> { 4, 6, 6, 4 };

        protected ushort _cellSize;
        protected ushort _cellPadding;

        protected bool _isPlaying = false;
        protected bool _orientation = false;

        protected Random _random = new Random();

        public delegate void EventAroundCLearHandler(EnumCellColor[,] map, int posX, int posY);
        public event EventAroundCLearHandler EventAroundClear;

        protected Color _userBackColor;
        protected Color _userFontColor;
        protected Font _userFontFont;

        public Color UserBackColor 
        {
            get { return _userBackColor; }
            set { _userBackColor = value; Invalidate(); }
        }
        public Color UserFontColor 
        {
            get { return _userFontColor; }
            set { _userFontColor = value; Invalidate(); }
        }
        public Font UserFontFont 
        {
            get { return _userFontFont; }
            set { _userFontFont = value; Invalidate(); }
        }
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
        protected ushort CellSize
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
        protected ushort CellPadding
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
        protected readonly Ship currentShip = new Ship(0, 0, 4, true);

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
            e.Graphics.FillRectangle(new SolidBrush(UserBackColor), backgroung);

            //отрисовка поля игрока
            DrawMap(e, _playerMap);

            //отрисовка поля бота
            DrawMap(e, _botMap);

            //Graphics g = e.Graphics;
            //Pen pen = new Pen(Color.Black);
            //g.DrawRectangle(pen, X, Y, width, height);

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
        protected void DrawMap(PaintEventArgs e, EnumCellColor[,] map)
        {
            Rectangle cellRectangle;
            Brush cellColor = new SolidBrush(Color.White);
            int cellSize = CellSize + CellPadding;
            //text
            Font font = UserFontFont;
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
                e.Graphics.DrawString((i + 1).ToString(), font, new SolidBrush(UserFontColor), cellRectangle, sf);
                //буковки
                cellRectangle = new Rectangle(cellSize * (i + int.Parse(bufVarMap)), _cellPadding, _cellSize, _cellSize);
                e.Graphics.DrawString(_aplhabet[i].ToString(), font, new SolidBrush(UserFontColor), cellRectangle, sf);
            }
        }

        //расстановка кораблей
        public EnumCellColor[,] ConfigureShips(bool valueWhoseMap)
        {
            EnumCellColor[,] map;

            currentShip.lengthShip = 4;
            currentShip.countShip();

            //_shipsCount = 10;

            //выборвка координат

            map = valueWhoseMap ? _playerMap : _botMap;

            ClearMap(map);

            while (currentShip.count <= 10)
            {
                ConfigureShipsSet(map);

                ++currentShip.count;
                --currentShip.lengthShip;
            }

            Normalized(map);
            Invalidate();
            return map;
        }
        //облегчение ConfigureShips
        protected void ConfigureShipsSet(EnumCellColor[,] map)
        {
            int begin = 0; int end = 10;

            for (int i = 0; i < currentShip.count; ++i)
            {

                _orientation = _random.Next(1, 101) % 2 == 0;

                currentShip.x = _random.Next(begin, end);
                currentShip.y = _random.Next(begin, end);

                int deltaX = _orientation ? currentShip.lengthShip - 1 : 0;
                int deltaY = !_orientation ? currentShip.lengthShip - 1 : 0;

                while (!IsInsideMap(currentShip.x + deltaY, currentShip.y + deltaX) ||
                    !IsEmpty(currentShip.x, currentShip.y, currentShip.lengthShip, _orientation, map))
                {
                    currentShip.x = _random.Next(begin, end);
                    currentShip.y = _random.Next(begin, end);
                }

                int curMaxShipOrientation = (_orientation ? currentShip.y : currentShip.x) + currentShip.lengthShip;
                for (int k = _orientation ? currentShip.y : currentShip.x; k < curMaxShipOrientation; ++k)
                {
                    if ((_orientation ? map[currentShip.x, k] : map[k, currentShip.y]) == EnumCellColor.UntouchedCell)
                        map[_orientation ? currentShip.x : k, _orientation ? k : currentShip.y] = (EnumCellColor)currentShip.lengthShip;
                }

                NerbaryShips(currentShip.x, currentShip.y, currentShip.lengthShip, _orientation, map);

                //--_shipsCount;
                //if (_shipsCount <= 0)
                //    break;
            }
        }

        protected enum EnumDir { Up = 0, Left, Down, Right }

        protected bool _woundedCell = false;
        
        //робот стрелять
        protected bool BotShoot()
        {
            bool hit = false;
            Random random = new Random();
            int x = random.Next(0, 10);
            int y = random.Next(0, 10);

            if (!_woundedCell)
            {
                while (_playerMap[x, y] == EnumCellColor.BackColor || _playerMap[x, y] == EnumCellColor.WoundedShip || _playerMap[x, y] == EnumCellColor.SlainShip)
                {
                    x = random.Next(0, 10);
                    y = random.Next(0, 10);
                }
            }
            else
            {
                for (int Row = 0; Row < 10; ++Row)
                {
                    for (int Col = 0; Col < 10; ++Col)
                    {
                        if (_playerMap[Row, Col] == EnumCellColor.WoundedShip)
                        {
                            _botBattleAIMap[Row, Col] = EnumCellColor.WoundedShip;
                        }

                        if (_playerMap[Row, Col] == EnumCellColor.SlainShip)
                        {
                            _botBattleAIMap[Row, Col] = EnumCellColor.SlainShip;
                        }

                        if (_playerMap[Row, Col] == EnumCellColor.BackColor)
                        {
                            _botBattleAIMap[Row, Col] = EnumCellColor.BackColor;
                        }
                    }
                }
                //считает раненную клетку в марках
                int[,] Marks = new int[10, 10];

                for (int Row = 0; Row < 10; ++Row)
                {
                    for (int Col = 0; Col < 10; ++Col)
                    {
                        if (_botBattleAIMap[Row, Col] == EnumCellColor.WoundedShip)
                        {
                            //Marks[Row, Col] = CalcRightMark(Row, Col) + CalcLeftMark(Row, Col) +
                            //    CalcUpMark(Row, Col) + CalcDownMark(Row, Col);
                            if (_botBattleAIMap[Row > 8 ? Row : Row + 1, Col] == EnumCellColor.UntouchedCell)
                                Marks[Row > 8 ? Row : Row + 1, Col] += CalcRightMark(Row, Col) + CalcLeftMark(Row, Col);
                            if (_botBattleAIMap[Row < 1 ? Row : Row - 1, Col] == EnumCellColor.UntouchedCell)
                                Marks[Row < 1 ? Row : Row - 1, Col] += CalcRightMark(Row, Col) + CalcLeftMark(Row, Col);
                            if (_botBattleAIMap[Row, Col < 1 ? Col : Col - 1] == EnumCellColor.UntouchedCell)
                                Marks[Row, Col < 1 ? Col : Col - 1] += CalcDownMark(Row, Col) + CalcUpMark(Row, Col);
                            if (_botBattleAIMap[Row, Col > 8 ? Col : Col + 1] == EnumCellColor.UntouchedCell)
                                Marks[Row, Col > 8 ? Col : Col + 1] += CalcDownMark(Row, Col) + CalcUpMark(Row, Col);
                        }
                    }
                }
                // Поиск максимальной оценки на поле
                int MaxMark = 0;
                for (int Row = 0; Row < 10; ++Row)
                {
                    for (int Col = 0; Col < 10; ++Col)
                    {
                        if (Marks[Row, Col] > MaxMark)
                            MaxMark = Marks[Row, Col];
                    }
                }
                // Построение списка клеток на поле, в которые, вероятнеев сего, необходимо выстрелить
                List<Cell> Shuts = new List<Cell>();
                for (int Row = 0; Row < 10; ++Row)
                {
                    for (int Col = 0; Col < 10; ++Col)
                    {
                        if (Marks[Row, Col] == MaxMark)
                            Shuts.Add(new Cell(Row, Col));
                    }
                }
                // Выбор клетки поля из списка случайным образом
                Random Generator = new Random(DateTime.Now.Millisecond);
                int ShutIndex = Generator.Next(Shuts.Count);
                
                x = Shuts[ShutIndex].Row;
                y = Shuts[ShutIndex].Col;
            }

            if (_playerMap[x, y] > EnumCellColor.UntouchedCell && _playerMap[x, y] < EnumCellColor.BackColor)
            {
                hit = true;
                _woundedCell = true;
                --playerlifeShips[4 - (int)_playerMap[x, y]];
                _playerMap[x, y] = (!NearCells(_playerMap, x, y) && playerlifeShips[4 - (int)_playerMap[x, y]] % (int)_playerMap[x, y] == 0) ? EnumCellColor.SlainShip : EnumCellColor.WoundedShip;

                if (_playerMap[x, y] == EnumCellColor.SlainShip)
                    EventAroundClear?.Invoke(_playerMap, x, y);
                
            }
            
            if (_playerMap[x, y] == EnumCellColor.UntouchedCell)
            {
                hit = false;
                _playerMap[x, y] = EnumCellColor.BackColor;
            }

            if (_playerMap[x, y] == EnumCellColor.SlainShip)
            {
                _woundedCell = false;
                //for (int Row = 0; Row < 10; ++Row)
                //{
                //    for (int Col = 0; Col < 10; ++Col)
                //    {
                if (_playerMap[x, y] == EnumCellColor.SlainShip)
                    _botBattleAIMap[x, y] = EnumCellColor.SlainShip;
                    //}
                //}
            }

            if (hit)
                BotShoot();
 
            return hit;
        }

        //оценка клетки поля
        protected int CalcRightMark(int Row, int Col)
        {
            int Result = 0;
            int Offset = 0;
            while (Row + Offset < 10 && Offset < 4 && _botBattleAIMap[Row + Offset, Col] == EnumCellColor.WoundedShip )
            {
                Result++;
                Offset++;
            }
            return Result;
        }

        protected int CalcLeftMark(int Row, int Col)
        {
            int Result = 0;
            int Offset = 0;
            while (Row - Offset >= 0 && Offset < 4 && _botBattleAIMap[Row - Offset, Col] == EnumCellColor.WoundedShip)
            {
                Result++;
                Offset++;
            }
            return Result;
        }

        protected int CalcDownMark(int Row, int Col)
        {
            int Result = 0;
            int Offset = 0;
            while (Col + Offset < 10 && Offset < 4 && _botBattleAIMap[Row, Col + Offset] == EnumCellColor.WoundedShip)
            {
                Result++;
                Offset++;
            }
            return Result;
        }

        protected int CalcUpMark(int Row, int Col)
        {
            int Result = 0;
            int Offset = 0;
            while (Col - Offset >= 0 && Offset < 4 && _botBattleAIMap[Row, Col - Offset] == EnumCellColor.WoundedShip)
            {
                Result++;
                Offset++;
            }
            return Result;
        }

        //очищение поля вокруг убитого корабля(пусто)
        protected void AroundClear(EnumCellColor[,] map, int posX, int posY)
        {
            Stack<KeyValuePair<Point, Point>> StackShip = new Stack<KeyValuePair<Point, Point>>();
            StackShip.Push(new KeyValuePair<Point, Point>(new Point(-1, -1), new Point(posX, posY)));

            try
            {
                do
                {
                    KeyValuePair<Point, Point> kv = StackShip.Pop();

                    Point parentPoint = kv.Key;
                    Point point = kv.Value;
                    for (int deltaX = -1; deltaX <= 1; ++deltaX)
                    {
                        for (int deltaY = -1; deltaY <= 1; ++deltaY)
                        {
                            if (deltaX == 0 && deltaY == 0) continue;
                            if (parentPoint.X == point.X + deltaX && parentPoint.Y == point.Y + deltaY) continue;

                            if (point.X + deltaX >= 0 &&
                                point.X + deltaX < 10 &&
                                point.Y + deltaY >= 0 &&
                                point.Y + deltaY < 10) // надо проверить
                            {
                                EnumCellColor currentMapPoint = map[point.X + deltaX, point.Y + deltaY];
                                if (currentMapPoint == EnumCellColor.WoundedShip)
                                    StackShip.Push(new KeyValuePair<Point, Point>(point, new Point(point.X + deltaX, point.Y + deltaY)));
                                else if (currentMapPoint == 0)
                                    map[point.X + deltaX, point.Y + deltaY] = EnumCellColor.BackColor;
                            }
                        }
                    }
                } while (StackShip.Peek().Key.X != -1);
            }
            catch { };
        }

        //стрельба логика
        public void Shoot(object sender, MouseEventArgs e)
        {
            if (isPlaying)
            {
                int X = e.X;
                int Y = e.Y;
                bool playerTurn = PlayerShoot(X, Y);
                if (!playerTurn)
                    BotShoot();

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
                    isPlaying = false;
                    _orientation = false;
                    _playerMap = new EnumCellColor[10, 10];
                    _botMap = new EnumCellColor[10, 10];
                    _botBattleAIMap = new EnumCellColor[10, 10];
                    _ship = new int[10, 10];
                    playerlifeShips = new List<int> { 4, 6, 6, 4 };
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
                    isPlaying = false;
                    _orientation = false;
                    _playerMap = new EnumCellColor[10, 10];
                    _botMap = new EnumCellColor[10, 10];
                    _botBattleAIMap = new EnumCellColor[10, 10];
                    _ship = new int[10, 10];
                    playerlifeShips = new List<int> { 4, 6, 6, 4 };
                }

                if (!CheckIfMapIsNotEmpty(_playerMap) || !CheckIfMapIsNotEmpty(_botMap))
                {
                    ClearMap(_botMap);
                    ClearMap(_playerMap);
                    isPlaying = false;
                    _orientation = false;
                    _playerMap = new EnumCellColor[10, 10];
                    _botMap = new EnumCellColor[10, 10];
                    _botBattleAIMap = new EnumCellColor[10, 10];
                    _ship = new int[10, 10];
                    playerlifeShips = new List<int> { 4, 6, 6, 4 }; 
                }
            }
            Invalidate();
        }

        //логика стрельбы игрока (norm)
        protected bool PlayerShoot(int X, int Y)
        {
            if (!isPlaying) return false;

            bool hit = false;

            int x = (Y - 30) / (_cellSize + _cellPadding); 
            int y = (X - 430) / (_cellSize + _cellPadding);

            if (!IsInsideMap(x, y)) return false; 
            else hit = true;
            
            if (_botMap[x, y] > EnumCellColor.UntouchedCell && _botMap[x, y] < EnumCellColor.BackColor)
            {
                hit = true;

                --currentShip.lifeShips[4 - (int)_botMap[x, y]];
                _botMap[x, y] = (!NearCells(_botMap, x, y) && currentShip.lifeShips[4 - (int)_botMap[x, y]] % (int)_botMap[x, y] == 0) ? EnumCellColor.SlainShip : EnumCellColor.WoundedShip;
            }
            else
            {
                if (_botMap[x, y] != EnumCellColor.BackColor && _botMap[x, y] != EnumCellColor.WoundedShip && _botMap[x, y] != EnumCellColor.SlainShip)
                {
                    hit = false;
                    _botMap[x, y] = EnumCellColor.BackColor;
                }
                else hit = true;
            }

            if (_botMap[x, y] == EnumCellColor.SlainShip)
                EventAroundClear?.Invoke(_botMap, x, y);

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

        //реализация расставления кораблей вручную  
        public void PlacementShipsPlayer(Point MousePos)
        {
            Point MousePos1 = PointToClient(MousePos);
            int _xCor = MousePos1.Y / (_cellSize + _cellPadding); // норм
            int _yCor = (MousePos1.X - 430) / (_cellSize + _cellPadding); // норм

            if (PlaceShip)
            {
                _xCor = MousePos1.Y / (_cellSize + _cellPadding) - 1;
                _yCor = (MousePos1.X) / (_cellSize + _cellPadding) - 1;

                if (IsInsideMap(_xCor, _yCor))
                {
                    currentShip.orientation = _orientation;
                    int valueShip = currentShip.lengthShip;

                    OrientationShip(_yCor, _xCor, valueShip, currentShip.orientation);

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
            }
        }

        //изменение ориентации по нажатию клавиши
        public void KeyOrientation(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z)
                Orientation = !Orientation;
        }

        //проверка ориентации
        protected bool OrientationShip(int i, int j, int valueShip, bool place)
        {
            if (IsInsideMap(i, j))
            {
                if (place)
                {
                    for (int count = valueShip; count != 0; ++j)
                    {
                        if (j + valueShip > 10)
                            j -= count + j - 10;

                        _playerMap[i, j] = (EnumCellColor)valueShip;
                        --count;
                            
                    }
                    return true;
                }
                else
                {
                    for (int count = valueShip; count != 0; ++i)
                    {
                        if (i + valueShip > 10)
                            i -= count + i - 10;

                        _playerMap[i, j] = (EnumCellColor)valueShip;
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
            if (!isPlaying) return false;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (map[i, j] > EnumCellColor.UntouchedCell && map[i, j] < EnumCellColor.BackColor)
                    {
                        return true;
                    }
                }
            }   
            return false;
        }

        //расстановка кораблей -1 вокруг
        protected void NerbaryShips(int posX, int posY, int lengthShip, bool orientation, EnumCellColor[,] map) 
        {
            //true - вертикальная
            if (orientation)
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

        //проверка на свободное место (убрать бы ориентатион этот)
        protected bool IsEmpty(int i, int j, int length, bool orientation, EnumCellColor[,] map)
        {
            for (int k = (_orientation ? j : i); k < (_orientation ? j : i) + length; ++k)
                if (_orientation ? map[i, k] != 0 : map[k, j] != 0)
                    return false;

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

        //расставить корабли на поле снизу
        protected void CreateCurrentShipPlayer(int x, int typeShip)
        {
            for (int i = 0; i <= x; ++i)
                for (int j = 0; j < typeShip; ++j)
                    _ship[i, j] = typeShip;

            Invalidate();
        }
    }
}
