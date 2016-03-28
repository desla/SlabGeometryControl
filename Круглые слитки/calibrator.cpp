#include <stdio.h>
#include <iostream>
#include <vector>
#include <algorithm>
#include <string>
#include <string.h>
#include <set>
#include <stack>
#include <queue>
#include <math.h>
#include <map>
#include <cmath>
#include <set>
#include <cstdlib>

using namespace std;
typedef long long ll;
typedef unsigned long long ull;

// начальные положения датчиков.
double xa = -19.0, // верхний датчик.
xb = -51.5, // левый датчик.
xc = -30.0; // нижний датчик.

// максимальный сдвиг датчиков влево и вправо, мм.
double shiftLimit = 100;

// шаг сдвига, мм.
double dx = 0.5;

// показания датчиков.
double va = 332.6389,
vb = 293.0556,
vc = 231.9444;

// расстояния между датчиками.
double dac = 672.133,
	   dbd = 735.327;

// диаметр болванки.
double sourceDiameter = 110.35;

struct point {
	double x, y;

	double distance(point t) {
		return sqrt((x - t.x)*(x - t.x) + (y - t.y)*(y - t.y));
	}
};

double getDiameter(point a, point b, point c) {
	double p1 = a.distance(b);
	double p2 = b.distance(c);
	double p3 = c.distance(a);
	double p = (p1 + p2 + p3) / 2.0;
	return (p1 * p2 * p3) / (2.0 * sqrt(p * (p - p1) * (p - p2) * (p - p3)));
}

int main()
{
#if !defined(ONLINE_JUDGE)
	freopen("input.txt", "r", stdin);
	freopen("output.txt ", "w", stdout);
#endif		

	double error = DBL_MAX;
	double resa, resb, resc;
	double resDiameter;

	for (double ta = xa - shiftLimit; ta <= xa + shiftLimit; ta += dx) {
		for (double tb = xb - shiftLimit; tb <= xb + shiftLimit; tb += dx) {
			for (double tc = xc - shiftLimit; tc <= xc + shiftLimit; tc += dx) {				
				point a, b, c;

				a.x = ta;
				a.y = dac / 2.0 - va;

				b.x = -dbd / 2.0 + vb;
				b.y = tb;

				c.x = tc;
				c.y = -dac / 2.0 + vc;

				double currentDiameter = getDiameter(a, b, c);
				double currentError = fabs(sourceDiameter - currentDiameter);
				if (currentError < error) {
					error = currentError;
					resDiameter = currentDiameter;
					resa = ta;
					resb = tb;
					resc = tc;
				}
			}
		}
	}

	printf("Смещения датчиков:\n");
	printf("A: %.3lf\nB: %.3lf\nC: %.3lf\n", resa, resb, resc);
	printf("Высчитанный диаметр: %.3lf\n", resDiameter);
	printf("Исходный диаметр: %.3lf\n", sourceDiameter);
	printf("Ошибка: %.3lf\n", error);	

	return 0;
}