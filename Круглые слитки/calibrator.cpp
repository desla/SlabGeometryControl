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
double xa = -15.8, // верхний датчик.
xb = -38.3, // левый датчик.
xc = -19.0, // нижний датчик.
xd = -62.5;   // правый датчик.

// максимальный сдвиг датчиков влево и вправо, мм.
double shiftLimit = 5;

// шаг сдвига, мм.
double dx = 0.1;

// показания датчиков.
double va = 332.6389, // верхний датчик.
vb = 293.0556, // левый датчик.
vc = 229.1667, // нижний датчик.
vd = 336.1111;  // правый датчик.

// расстояния между датчиками.
double dac = 672.133,
dbd = 735.327;

// диаметр болванки.
double sourceDiameter = 110.453;

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

void calcCircleCenter(point aA, point aB, point aC, point &center) {	
	double x1 = aA.x, x2 = aB.x, x3 = aC.x,
		y1 = aA.y, y2 = aB.y, y3 = aC.y;
	double x12 = x1 - x2,
		x23 = x2 - x3,
		x31 = x3 - x1,
		y12 = y1 - y2,
		y23 = y2 - y3,
		y31 = y3 - y1;
	double z1 = x1 * x1 + y1 * y1,
		z2 = x2 * x2 + y2 * y2,
		z3 = x3 * x3 + y3 * y3;
	double zx = y12 * z3 + y23 * z1 + y31 * z2,
		zy = x12 * z3 + x23 * z1 + x31 * z2,
		z = x12 * y31 - y12 * x31;
	
	center.x = -zx / (2.0 * z);
	center.y = zy / (2.0 * z);
}

int main()
{
	setlocale(LC_ALL, "Russian");

	double error = DBL_MAX;
	double resa, resb, resc;
	double resDiameter;
	point resCenter;

	double stepsPerSensor = 2.0 * shiftLimit / dx;
	double stepsCount = pow(stepsPerSensor, 3.0);

	// вычисляем смещения верхнего, левого и нижнего датчиков.
	int stepNumber = 0;
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
					calcCircleCenter(a, b, c, resCenter);
				}
			}			
		}

		if (stepNumber % 10 == 0) {
			system("cls");
			printf("Завершено: %.1lf %%\n", 100 * stepNumber * stepsPerSensor * stepsPerSensor / stepsCount);
		}

		stepNumber++;
	}

	double minDifference = DBL_MAX;
	double resd;

	// вычисляем смещения правого датчика.
	for (double td = xd - shiftLimit; td <= xd + shiftLimit; td += dx) {
		point d;
		d.x = dbd / 2.0 - vd;
		d.y = td;

		double distance = resCenter.distance(d);
		double currentDifference = fabs(distance - sourceDiameter / 2.0);
		if (currentDifference < minDifference) {
			minDifference = currentDifference;
			resd = td;
		}
	}

	system("cls");
	printf("Завершено: 100 %%\n");

	FILE * output = fopen("output.txt", "w");
	fprintf(output, "Смещения датчиков:\n");
	fprintf(output, "Верхний: %.3lf\n", resa);
	fprintf(output, "Левый: %.3lf\n", resb);
	fprintf(output, "Нижний: %.3lf\n", resc);
	fprintf(output, "Правый: %.3lf\n", resd);
	fprintf(output, "\n");
	fprintf(output, "Высчитанный диаметр: %.3lf\n", resDiameter);
	fprintf(output, "Исходный диаметр: %.3lf\n", sourceDiameter);
	fprintf(output, "Ошибка диаметра: %.3lf\n", error);
	fprintf(output, "Ошибка правого датчика: %.3lf\n", minDifference);
	fclose(output);

	return 0;
}