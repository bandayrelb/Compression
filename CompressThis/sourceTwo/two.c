/*
Name: Bernadette Bandayrel
Purpose: Write a C++ program for a video shop to handle some of the basic operation in the shop. Create a struct movieItem to record the
movie title, the number of available copies of this movie and the type of the movie.

In your program, it is also important to keep information about customers. Therefore, create a struct customer to hold customer’s name, the 
list of movies rented by this customer (array of type movieItem with maximum size 10) and the number of rented movies by the customer.

Your program must contain the following functions :

- A function that takes as parameter myMovie of type movieItem. The function should prompt the user to enter the movie details.
- A function that takes as parameter myMovie of type movieItem. The function should prints the movie details.
- A function named outOfStock that takes as parameters an array of movies, and the number of movies in the array. The function should search
the List array and print out the titles of all the movies in the List which are out of stock (number of copies equals zero).
- A function that takes as parameter myCustomer of type customer. The function should print the titles of all movies rented bymyCustomer.

Test your program with samples of movies and customer details. Suppose the video shop has 20 movies, and ask the user for the number of 
customers. Use an array of 20 components of type movieItem, and a dynamic array of type customer.
*/

#include "stdio.h"

#define MAX 2

typedef struct movieItem
{
	char title[30];
	int copies;
	char genre[30];
};

typedef struct customer
{
	char name[30];
	struct movieItem list[MAX];
	int numberRented;
};

void movieInfo(struct movieItem info[]);
void printInfo(struct movieItem info[]);
void outOfStock(struct movieItem info[]);
void myCustomer(struct customer info[]);

void main()
{
	struct movieItem info[MAX];
	struct customer data[MAX];

	movieInfo(info);
	printInfo(info);
	myCustomer(data);
	outOfStock(info);
}

void movieInfo(struct movieItem info[])
{
	for(int i = 0; i < MAX; i++)
	{
		printf("Enter name of movie:");
		scanf_s("%s", info[i].title, 29);

		printf("Enter number of copies:");
		scanf_s("%d", &info[i].copies);

		printf("Enter genre of movie:");
		scanf_s("%s", info[i].genre, 29);
	}
}

void printInfo(struct movieItem info[])
{
	for(int i = 0; i < MAX; i++)
	{
		printf("Title: %s\n", info[i].title);
		printf("Genre: %s\n", info[i].genre);
		printf("Copies Left: %d\n", info[i].copies);
	}
}

void myCustomer(struct customer data[])
{
	int number;

	printf("Enter number of customers:");
	scanf_s("%d", &number);

	for(int i = 0; i < number; i++)
	{
		printf("Enter customer's name:");
		scanf_s("%s", data[i].name, 29);

		
		printf("Enter movie rented:");
		scanf_s("%s", data[i].list, 29);

		printf("Enter number of movies rented total:");
		scanf_s("%d", &data[i].numberRented);
	}

	for(int i = 0; i < number; i++)
	{
		printf("Name: %s\n", data[i].name);

		printf("Movie Rented: %s", data[i].list);
		
		printf("\n");
		printf("Total Rented: %d\n", data[i].numberRented);
	}
}

void outOfStock(struct movieItem info[])
{
	for(int i = 0; i < MAX; i++)
	{
		if(info[i].copies == 0)
		{
			printf("%s is out of stock.\n", info[i].title);
		}
	}
}