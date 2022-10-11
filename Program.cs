using static System.Math;

double A = 0, B = 0, C = 0;

double cubeWidth = 20;
int width = 80, height = 22;
double[] zBuffer = new double[width * height];
char[] buffer = new char[width * height];

// 32
int backgroundASCIICode = ' ';
double incrementSpeed = 0.16;
int distanceFromCam = 120;
float K1 = 40;

double x, y, z;
double ooz;
int xp, yp;
int idx;

double calculateX(int i, int j, int k)
{
    return  j * Sin(A) * Sin(B) * Cos(C) - k * Cos(A) * Sin(B) * Cos(C) +
            j * Cos(A) * Sin(C) + k * Sin(A) * Sin(C) + i * Cos(B) * Cos(C);
}

double calculateY(int i, int j, int k)
{
    return  j * Cos(A) * Cos(C) + k * Sin(A) * Cos(C) - 
            j * Sin(A) * Sin(B) * Sin(C) + k * Cos(A) * Sin(B) * Sin(C) -
            i * Cos(B) * Sin(C);
}

double calculateZ(int i, int j, int k)
{
    return k * Cos(A) * Cos(B) - j * Sin(A) * Cos(B) + i * Sin(B);
}

void calculateForSurface(double cubeX, double cubeY, double cubeZ, char ch)
{
    x = calculateX((int)cubeX, (int)cubeY, (int)cubeZ);
    y = calculateY((int)cubeX, (int)cubeY, (int)cubeZ);
    z = calculateZ((int)cubeX, (int)cubeY, (int)cubeZ) + distanceFromCam;

    ooz = 1 / z;

    xp = (int)(width / 2 + K1 * ooz * x * 2);
    yp = (int)(height / 2 + K1 * ooz * y);

    idx = xp + yp * width;
    if (idx >= 0 && idx < width * height)
    {
        double bufferValue = zBuffer[idx];
        if (ooz > bufferValue)
        {
            zBuffer[idx] = ooz;
            buffer[idx] = ch;
        }
    }
}

while (true)
{
    Console.CursorVisible = false;
    Console.Clear();
    Array.Fill<char>(buffer, (char)backgroundASCIICode);
    Array.Fill<double>(zBuffer, 0);// width * height * 4);
    for (double cubeX = -cubeWidth; cubeX < cubeWidth; cubeX += incrementSpeed)
    {
        for (double cubeY = -cubeWidth; cubeY < cubeWidth; cubeY += incrementSpeed)
        {
            calculateForSurface(cubeX, cubeY, -cubeWidth, '.');
            calculateForSurface(cubeWidth, cubeY, cubeX, '$');
            calculateForSurface(-cubeWidth, cubeY, -cubeX, '~');
            calculateForSurface(-cubeX, cubeY, cubeWidth, '#');
            calculateForSurface(cubeX, -cubeWidth, -cubeY, ';');
            calculateForSurface(cubeX, cubeWidth, cubeY, '-');
        }
    }
    
    Console.CursorLeft = 0;

    for (int k = 0; k < width * height; k++)
    {
        if (k % width == 0)
        {
            Console.WriteLine();
        }
        else
        {
            Console.Write(buffer[k]);
        }

        A += 0.0005;
        B += 0.0005;

    }
    Thread.Sleep(30);
}