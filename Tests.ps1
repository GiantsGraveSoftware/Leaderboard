# Post without api key - should fail
Invoke-WebRequest -Uri https://localhost:5000/adduser -Method POST -Body '{"name":"John","email":"John@beatles.com","password":"Lennon"}' -ContentType "application/json"

# Same request with a valid api key - should work
# Run this a second time to get a (409) Conflict if the user already exists 
Invoke-WebRequest -Uri https://localhost:5000/adduser -Method POST -Body '{"ApiKey":"My New Api Key","name":"John","email":"John@beatles.com","password":"Lennon"}' -ContentType "application/json"

# Add some more users
Invoke-WebRequest -Uri https://localhost:5000/adduser -Method POST -Body '{"ApiKey":"My New Api Key","name":"Paul","email":"Paul@beatles.com","password":"McCartney"}' -ContentType "application/json"
Invoke-WebRequest -Uri https://localhost:5000/adduser -Method POST -Body '{"ApiKey":"My New Api Key","name":"George","email":"George@beatles.com","password": "Harrison"}' -ContentType "application/json"
Invoke-WebRequest -Uri https://localhost:5000/adduser -Method POST -Body '{"ApiKey":"My New Api Key","name":"Ringo","email":"Ringo@beatles.com","password": "Starr"}' -ContentType "application/json"

# Demonstrate a pre hashed user
Invoke-WebRequest -Uri https://localhost:5000/adduser -Method POST -Body '{"ApiKey":"My New Api Key","name":"Pre Hashed Test","hashemail":"My Pre-Hashed email string","hashpassword":"My Pre-Hashed password string"}' -ContentType "application/json"

# Login without password for error, with password to get JSON response
Invoke-WebRequest -Uri https://localhost:5000/userlogin -Method POST -Body '{"ApiKey":"My New Api Key","email":"John@beatles.com","password":"WrongPassword"}' -ContentType "application/json"
Invoke-WebRequest -Uri https://localhost:5000/userlogin -Method POST -Body '{"ApiKey":"My New Api Key","email":"John@beatles.com","password":"Lennon"}' -ContentType "application/json"

# Update user - change name from John to Jack and update their email address
Invoke-WebRequest -Uri https://localhost:5000/userupdate -Method POST -Body '{"ApiKey":"My New Api Key","ID":1, "Name":"Jack"}' -ContentType "application/json"
Invoke-WebRequest -Uri https://localhost:5000/userlogin -Method POST -Body '{"ApiKey":"My New Api Key","email":"John@beatles.com","password":"Lennon"}' -ContentType "application/json"
