# KiwiLib – Solution Documentation
Last updated : 25 Aug 2025
Updated by   : Vi Vo
-------------------------------------------------------------------
1. Design decision
	- This project is built on .NET8
	- This is design on 3 layers model
	- Library/API is asynchronous to make future multi-user expansion straightforward.
	- DataStore: Create local variables within DAL layer to store data for Books, Author, and Categories
	- Test framework: MSTest
	
2. Structure of project
   This project is designed on a 3-layer model:
   
	- KiwiLib.DAL: Is data access layer, interact directly with data store. In this example, data is stored in list variables within this library.
		There are 3 classes stand for 3 different tables to store Books, Author and Catogery information:
			AuthorRepository
			BookRepository
			CategoryRepository
		
	- KiwiLib.Services: Handle the core business logics including
		> Add book
		> Update book
		> List all books
		> Delete existing book
		> View book detail
		> Search book
		> ISBN validation and other checks
		> An extra method to auto generate a set of data for demo
		
	- KiwiLib: is the console interface (presentation layer) to interact with users
	
	- KiwiLib.Dto: Contain the objects definitions (Book, Author, Category) which are used accross the solution.
	
	- KiwiLib.Tests: Unit test projects for testing DAL and Business layers
	    > Please note, due to limit of time, only first few test methods were implemented, other testcases are intentional left blank for demo purpose 
