using System.Drawing;
using System.Runtime.CompilerServices;
using Tao.FreeGlut;
using Tao.OpenGl;

class Program
{
    const float Pi = 3.1415f;
    static float tx = 0;
    static float ty = 0;
    static float sx = 0.5f;
    static float sy = 0.5f;

    static float tx2 = 30;
    static float ty2 = 30;

    static float sinVariantA = 0;
    static float sinVariantB = 0;
    static float sinVariantC = 0;

    static void inicializa()
    {
        Gl.glClearColor(1.0f, 1.0f, 1.0f, 0.0f);
        Gl.glMatrixMode(Gl.GL_MODELVIEW);
        Gl.glLoadIdentity();
        Glu.gluOrtho2D(0f, 200f, 0f, 100f);
    }

    static void desenha()
    {
        Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
        Gl.glColor3f(0.0f, 0.0f, 0.0f);

        drawSky();
        drawSea();

        ColorObj[] boatColors =
        {
            new ColorObj(0.9f, 0.9f, 0.9f),          // juntador duas velas
            new ColorObj(0.9f, 0.9f, 0.9f),          // mastro principal
            new ColorObj(0.9f, 0.9f, 0.9f),          // mastro secundario
            new ColorObj(1f, 1f, 1f), // vela principal
            new ColorObj(1f, 1f, 1f), // vela secundaria
            new ColorObj(0.2f, 0.2f, 0.5f),       // casco superior
            new ColorObj(0.85f, 0.85f, 0.85f)        // casco inferior
        };

        movingBoatWrapper(50 + sinVariantA * 50, 61, 0.13f, 0.13f, boatColors);
        movingBoatWrapper(50 + sinVariantC * 50, 55, 0.15f, 0.15f, boatColors);
        movingBoatWrapper(50 + sinVariantA * 50, 47, 0.17f, 0.17f, boatColors);
        movingBoatWrapper(50 + sinVariantB * 50, 40, 0.20f, 0.20f, boatColors);
        movingBoatWrapper(50 + sinVariantC * 50, 30, 0.25f, 0.25f, boatColors);

        buoyWrapper(45, 25, 0.5f, 0.7f);

        buoyWrapper(50, 65, 0.2f, 0.3f);

        drawCircle(150, 85, 5f, new ColorObj(1,1,0)); //sol

        movingBoatWrapper(tx, ty, sx, sy, boatColors);

        Glut.glutSwapBuffers();
    }

    static void drawEllipse(float cx, float cy, float rx, float ry, int num_segments, bool tracadoHorario)
    {
        float theta = 2 * Pi / num_segments;
        float c = (float)Math.Cos(theta);
        float s = (float)Math.Sin(theta);
        float t;

        int signal = (tracadoHorario) ? -1 : 1;

        float x = 0;
        float y = -1;
        Gl.glColor3f(1.0f, 0.0f, 0.0f);
        Gl.glBegin(Gl.GL_LINE_LOOP);
        for (int i = 0; i < num_segments; i++)
        {
            Gl.glVertex2f(x * rx + cx, y * ry + cy);
            if (i == 1 || i == 0)
            {
                Gl.glColor3f(0f, 1f, 0f);
            }
            else {
                Gl.glColor3f(0.0f, 0.0f, 1.0f);
            }

            t = x;
            x = c * x - signal * s * y;
            y = signal * s * t + c * y;
        }
        Gl.glEnd();
    }

    static void buoyWrapper(float _tx, float _ty, float _sx, float _sy)
    {
        Gl.glPushMatrix();

        Gl.glTranslatef(_tx, _ty, 0);
        Gl.glScalef(_sx, _sy, 1);

        drawBuoy();

        Gl.glPopMatrix();
    }

    static void drawBuoy()
    {
        // base do buoy
        Gl.glColor3f(1f, 0f, 0f);
        Gl.glBegin(Gl.GL_POLYGON);
        Gl.glVertex2f(1, 0);
        Gl.glVertex2f(9, 0);
        Gl.glVertex2f(10, 3);
        Gl.glVertex2f(0, 3);
        Gl.glEnd();

        //corpo de baixo do buoy
        Gl.glLineWidth(8);
        Gl.glBegin(Gl.GL_LINES);
        Gl.glVertex2f(2,3);
        Gl.glVertex2f(4,8);
        Gl.glVertex2f(5,3);
        Gl.glVertex2f(5,8);
        Gl.glVertex2f(8,3);
        Gl.glVertex2f(6,8);
        Gl.glVertex2f(2,8);
        Gl.glVertex2f(8,8);
        Gl.glEnd();
        Gl.glLineWidth(1);

        drawCircle(5, 9, 1, new ColorObj(1,0,0));
    }

    static void drawSky()
    {
        Gl.glBegin(Gl.GL_POLYGON);

        Gl.glColor3f(0.647f, 0.874f, 0.964f);
        Gl.glVertex2f(0, 70);
        Gl.glVertex2f(200, 70);

        Gl.glColor3f(0.329f,0.552f,0.721f);
        Gl.glVertex2f(200, 100);
        Gl.glVertex2f(0, 100);

        Gl.glEnd();
    }

    static void drawSea()
    {
        Gl.glBegin(Gl.GL_POLYGON);
        Gl.glColor3f(0,0.8f,1f);

        Gl.glVertex2f(0,0);
        Gl.glVertex2f(0,70);
        Gl.glVertex2f(200,70);
        Gl.glVertex2f(200,0);

        Gl.glEnd();
    }

    static void movingBoatWrapper(float _tx, float _ty, float _sx, float _sy, ColorObj[] _colorObjArr)
    {
        Gl.glPushMatrix();

        Gl.glTranslatef(_tx, _ty, 0);
        Gl.glScalef(_sx, _sy, 1);

        drawBoat(_colorObjArr[0], _colorObjArr[1], _colorObjArr[2], _colorObjArr[3], _colorObjArr[4], _colorObjArr[5], _colorObjArr[6]);

        Gl.glPopMatrix();
    }

    static void drawBoat(
        ColorObj ColorJuntadorDuasVelas,
        ColorObj ColorMastroPrincipal,
        ColorObj ColorMastroSecundario,
        ColorObj ColorVelaPrincipal,
        ColorObj ColorVelaSecundaria,
        ColorObj ColorCascoSuperior,
        ColorObj ColorCascoInferior
        )
    {
        //juntador de duas velas
        Gl.glBegin(Gl.GL_POLYGON);
        //Gl.glColor3f(1.0f, 1.0f, 0.0f);
        Gl.glColor3f(ColorJuntadorDuasVelas.r, ColorJuntadorDuasVelas.g, ColorJuntadorDuasVelas.b);

        Gl.glVertex2f(43, 18);
        Gl.glVertex2f(42, 18);
        Gl.glVertex2f(40, 19.5f);
        Gl.glVertex2f(45, 19.5f);

        Gl.glEnd();

        //mastro principal
        Gl.glBegin(Gl.GL_POLYGON);
        //Gl.glColor3f(1.0f, 0.0f, 1.0f);
        Gl.glColor3f(ColorMastroPrincipal.r, ColorMastroPrincipal.g, ColorMastroPrincipal.b);

        Gl.glVertex2f(40.5f, 20);
        Gl.glVertex2f(42, 18);
        Gl.glVertex2f(42, 15);
        Gl.glVertex2f(40.5f, 15);

        Gl.glEnd();

        //mastro secundario
        Gl.glBegin(Gl.GL_POLYGON);
        //Gl.glColor3f(0.0f, 0.0f, 1.0f);
        Gl.glColor3f(ColorMastroSecundario.r, ColorMastroSecundario.g, ColorMastroSecundario.b);

        Gl.glVertex2f(43.5f, 18);
        Gl.glVertex2f(45, 20);
        Gl.glVertex2f(45, 15);
        Gl.glVertex2f(43.5f, 15);

        Gl.glEnd();

        //vela big
        Gl.glBegin(Gl.GL_POLYGON);
        //Gl.glColor3f(1.0f, 0.0f, 0.0f);
        Gl.glColor3f(ColorVelaPrincipal.r, ColorVelaPrincipal.g, ColorVelaPrincipal.b);

        Gl.glVertex2f(20, 20);
        Gl.glVertex2f(39, 56);
        Gl.glVertex2f(42, 18);

        Gl.glEnd();

        //smol vela
        Gl.glBegin(Gl.GL_POLYGON);
        //Gl.glColor3f(1.0f, 0.0f, 0.0f);
        Gl.glColor3f(ColorVelaSecundaria.r, ColorVelaSecundaria.g, ColorVelaSecundaria.b);

        Gl.glVertex2f(40, 44);
        Gl.glVertex2f(56, 17);
        Gl.glVertex2f(43, 18);

        Gl.glEnd();

        //casco
        Gl.glBegin(Gl.GL_POLYGON);
        //Gl.glColor3f(1.0f, 0.0f, 0.0f);
        Gl.glColor3f(ColorCascoSuperior.r, ColorCascoSuperior.g, ColorCascoSuperior.b);

        Gl.glVertex2f(10, 18);
        Gl.glVertex2f(62, 15);
        Gl.glVertex2f(62, 13);
        Gl.glVertex2f(14, 15);

        Gl.glEnd();

        Gl.glBegin(Gl.GL_POLYGON);
        //Gl.glColor3f(0.0f, 1.0f, 0.0f);
        Gl.glColor3f(ColorCascoInferior.r, ColorCascoInferior.g, ColorCascoInferior.b);

        Gl.glVertex2f(14, 15);
        Gl.glVertex2f(62, 13);
        Gl.glVertex2f(60, 10);
        Gl.glVertex2f(23, 10);

        Gl.glEnd();
    }

    static void drawCircle(float dx, float dy, float raio = 4f, ColorObj colorObj = null)
    {
        float x, y, pontos;
        pontos = (2 * Pi) / 1600;
        colorObj = (colorObj == null) ? new ColorObj(1,1,1) : colorObj;
        Gl.glColor3f(colorObj.r, colorObj.g, colorObj.b);
        Gl.glBegin(Gl.GL_TRIANGLE_FAN);
        for (float angulo = 0.0f; angulo <= 2 * Pi; angulo += pontos)
        {
            x = (float)(raio * Math.Cos(angulo) + dx);
            y = (float)(raio * Math.Sin(angulo) + dy);
            Gl.glVertex2f(x, y);
        }
        Gl.glEnd();

        //Glut.glutSwapBuffers();
    }

    static void keyboardControls(int key, int x, int y)
    {
        switch(key)
        {
            case Glut.GLUT_KEY_UP:
                ty += 0.7f;
                sx -= 0.005f;
                sy -= 0.005f;
                break;
            case Glut.GLUT_KEY_DOWN:
                ty -= 0.7f;
                sx += 0.005f;
                sy += 0.005f;
                break;
            case Glut.GLUT_KEY_LEFT:
                tx -= 0.7f;
                break;
            case Glut.GLUT_KEY_RIGHT:
                tx += 0.7f;
                break;
        }
        Glut.glutPostRedisplay();
    }

    static void TimerA(int angle)
    {
        // posição verdadeira = ponto inicial * função de variação, atualiza ~60 vezes por segundo
        double radianValue = angle * Math.PI / 180;
        sinVariantA = (float)(Math.Sin(radianValue) + 1);

        angle = (angle + 1) % 360;

        Glut.glutPostRedisplay();

        Glut.glutTimerFunc(16, TimerA, angle);
    }

    static void TimerB(int angle)
    {
        // posição verdadeira = ponto inicial * função de variação, atualiza ~40 vezes por segundo
        double radianValue = angle * Math.PI / 180;
        sinVariantB = (float)(Math.Sin(radianValue) + 1);

        angle = (angle + 1) % 360;

        Glut.glutPostRedisplay();

        Glut.glutTimerFunc(24, TimerB, angle);
    }

    static void TimerC(int angle)
    {
        // posição verdadeira = ponto inicial * função de variação, atualiza ~30 vezes por segundo
        double radianValue = angle * Math.PI / 180;
        sinVariantC = (float)(Math.Sin(radianValue) + 1);

        angle = (angle + 1) % 360;

        Glut.glutPostRedisplay();

        Glut.glutTimerFunc(32, TimerC, angle);
    }

    static void Main()
    {
        Glut.glutInit();
        Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_RGB);
        
        Glut.glutCreateWindow("Projeto Barquinho");
        Glut.glutReshapeWindow(1800, 900);
        inicializa();
        Glut.glutDisplayFunc(desenha);
        Glut.glutSpecialFunc(keyboardControls);
        Glut.glutTimerFunc(16, TimerA, 180);
        Glut.glutTimerFunc(24, TimerB, 180);
        Glut.glutTimerFunc(32, TimerC, 180);
        Glut.glutMainLoop();
    }
}

public class ColorObj
{
    public float r;
    public float g;
    public float b;
    public ColorObj(float r, float g, float b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }
}
