import mysql.connector

# database credentials
db_config = {
    'user': 'root',
    'password': 'Herman_02282003',
    'host': 'localhost',
    'database': 'WORLD',
}

# Establish a connection to the MySQL database
connection = mysql.connector.connect(**db_config)

# Create a cursor object to execute SQL queries
cursor = connection.cursor()

try:
    # Question: Insert a new country with a new language and at least one city, the capital, into the database
    # Insert 1
    insert_query_1 = """INSERT INTO country(Code, Name, Continent, Region, SurfaceArea, IndepYear, Population, 
        LifeExpectancy, GNP, GNPOld, LocalName, GovernmentForm, HeadOfState, Capital, Code2) 
        VALUES('AUZ', 'Auztralia', 'Oceania', 'Auztralia & New Zealand', 7692024.00, 1901, 25687041, 83.4, 
        1391670, 1379200, 'Auztralia', 'Constitutional Monarchy, Federation', 'King Charles III', 135, 'AU')"""
    cursor.execute(insert_query_1)

    # Insert 2
    insert_query_2 = """INSERT INTO city(Name, CountryCode, District, Population) 
        VALUES('Canberra', 'AUZ', 'ACT', 427000)"""
    cursor.execute(insert_query_2)

    # Insert 3
    insert_query_3 = """INSERT INTO countrylanguage(CountryCode, Language, IsOfficial, Percentage) 
        VALUES('AUZ', 'English', 'T', 72.0)"""
    cursor.execute(insert_query_3)

    # Commit the transaction
    connection.commit()
    print("Transaction completed successfully.")

except mysql.connector.Error as error:
    # Rollback the transaction if there's an error
    connection.rollback()
    print(f"Transaction failed. Rolled back due to: {error}")

# Close the cursor and connection
cursor.close()
connection.close()