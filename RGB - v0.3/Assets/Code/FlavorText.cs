using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlavorText
{
    public string[] firstNames = {
    "James", "John", "Robert", "Michael", "William",
    "David", "Richard", "Joseph", "Thomas", "Charles",
    "Christopher", "Daniel", "Matthew", "Anthony", "Donald",
    "Mark", "Paul", "Steven", "Andrew", "Kenneth",
    "George", "Joshua", "Kevin", "Brian", "Edward",
    "Ronald", "Timothy", "Jason", "Jeffrey", "Ryan",
    "Jacob", "Gary", "Nicholas", "Eric", "Stephen",
    "Jonathan", "Larry", "Justin", "Scott", "Brandon",
    "Frank", "Benjamin", "Gregory", "Samuel", "Raymond",
    "Patrick", "Alexander", "Jack", "Dennis", "Jerry",
    "Tyler", "Aaron", "Henry", "Douglas", "Peter",
    "Jose", "Adam", "Zachary", "Walter", "Kyle",
    "Harold", "Carl", "Jeremy", "Keith", "Roger",
    "Gerald", "Ethan", "Arthur", "Terry", "Christian",
    "Sean", "Lawrence", "Austin", "Joe", "Noah",
    "Jesse", "Albert", "Bryan", "Billy", "Bruce",
    "Willie", "Jordan", "Dylan", "Alan", "Ralph",
    "Gabriel", "Roy", "Juan", "Wayne", "Eugene",
    "Logan", "Randy", "Louis", "Russell", "Vincent",
    "Philip", "Bobby", "Johnny", "Bradley", "Anna",
    "Emily", "Isabella", "Samantha", "Elizabeth", "Sophia",
    "Alyssa", "Grace", "Sarah", "Natalie", "Megan",
    "Kayla", "Jennifer", "Lauren", "Victoria", "Addison",
    "Destiny", "Sydney", "Rachel", "Lily", "Alexis",
    "Hannah", "Ashley", "Brianna", "Olivia", "Emma",
    "Madison", "Hailey", "Taylor", "Nicole", "Jasmine",
    "Morgan", "Leah", "Kimberly", "Mackenzie", "Stephanie",
    "Courtney", "Isabel", "Melissa", "Sophie", "Amelia",
    "Kaitlyn", "Rebecca", "Mary", "Zoe", "Julia",
    "Allison", "Laura", "Gabriella", "Brooke", "Amanda",
    "Amber", "Paige", "Andrea", "Sierra", "Katherine",
    "Michelle", "Sara", "Jessica", "Lillian", "Vanessa",
    "Jordan", "Alexandra", "Jenna", "Maria", "Trinity",
    "Madeline", "Eva", "Ariana", "Faith", "Autumn",
    "Melanie", "Bailey", "Naomi", "Caroline", "Claire",
    "Isabelle", "Aaliyah", "Molly", "Alexa", "Angela",
    "Kate", "Sadie", "Nicole", "Mia", "Charlotte",
    "Sophia", "Zoey", "Lily", "Emily", "Madelyn",
    "Chloe", "Layla", "Olivia", "Ella", "Riley",
    "Amelia", "Grace", "Ava", "Isabella", "Aubrey",
    "Harper", "Addison", "Elizabeth", "Natalie", "Zoe",
    "Leah", "Brooklyn", "Lucy", "Audrey", "Anna",
    "Samantha", "Ariana", "Allison", "Savannah", "Arianna",
    "Camila", "Penelope", "Gabriella", "Claire", "Aaliyah",
    "Sadie", "Riley", "Skylar", "Nora", "Sarah",
    "Hailey", "Kaylee", "Paisley", "Kennedy", "Ellie",
    "Peyton", "Annabelle", "Caroline", "Madeline", "Serenity",
    "Aubree", "Lucia", "Maya", "Isabelle", "Mackenzie",
    "Valentina", "Ruby", "Sophie", "Alice", "Kylie",
    "Maria", "Khloe", "Alexandra", "Violet", "Stella",
    "Katherine", "Gianna", "Alexa", "Charlie", "Julia",
    "Emery", "Juliana", "Ariella", "Scarlett", "Isabel",
    "Quinn", "Cora", "Aurora", "Elena", "London",
    "Jane", "Kinsley", "Ivy", "Lydia", "Nova",
    "Eliana", "Vivian", "Kayla", "Brielle", "Liliana",
    "Melody", "Madilyn", "Sara", "Alana", "Lila",
    "Athena", "Makayla", "Kali", "Willow", "Reagan",
    "Jordyn", "Hazel", "Margaret", "Ximena", "Blake",
    "Lola", "Mariah", "Alaina", "June", "Juliette",
    "Michelle", "Arabella", "Valeria", "Gia", "Mary",
    "Ashlynn", "Isla", "Laila", "Eden", "Ayla",
    "Julianna", "Leila", "Alina", "Keira", "Adalyn",
    "Daniela", "Presley", "Harlow", "Dakota", "Adelyn",
    "Megan", "Eloise", "Remi", "Reese", "Elise",
    "Eliza", "Alivia", "Joy", "Emersyn", "Mya",
    "Erica", "Mckenzie", "Tessa", "Makenzie", "Camille",
    "Nina", "Logan", "Adriana", "Amina", "Amara",
    "Anne", "Miriam", "Esther", "Diana", "Sloane",
    "Anastasia", "Bianca", "Cecilia", "Phoebe", "Heidi",
    "Fiona", "Georgia", "Lena", "Daphne", "Adelaide",
    "Celia", "Evangeline", "Elaina", "Camilla", "Sage",
    "Elsie", "Myla", "Melissa", "Louise", "Talia",
    "Maeve", "Ruth", "Alessandra", "Gwendolyn", "Olive",
    "Veronica", "Gemma", "Noelle", "Christina", "Elle",
    "Clarissa", "Lana", "Angelica", "Haley", "Leia",
    "Harmony", "Rowan", "Marilyn", "Carmen", "Karen",
    "Tabitha", "Ophelia", "Lennox", "Mallory", "Clara",
    "Mabel", "Sienna", "Wren", "Raegan", "Yara",
    "Kendra", "Nyla", "Christine", "Estelle", "Zara",
    "Gloria", "Clementine", "Monica", "Briana", "Freya",
    "Paula", "Danna", "Josie", "Marie", "Alondra",
    "Carla", "Fernanda", "Teresa", "Selena", "Eileen",
    "Lara", "Yasmin", "Imani", "Regina", "Lillie",
    "Anahi", "Rosa", "Nia", "Deborah", "Antonia",
    "Esmeralda", "Karla", "Arielle", "Emilie", "Erica",
    "Veda", "Whitney", "Amelie", "Ryan", "Nathalie",
    "Giuliana", "Mikayla", "Siena", "Elsa", "Jessica",
    "Nadia", "Alia", "Kenna", "Itzel", "Phoenix",
    "Lizbeth", "Fatima", "Alma", "Zuri", "Aileen",
    "Heaven", "April", "Elisa", "Constance", "Rosemary",
    "Galilea", "Marina", "Kailani", "Liberty", "Averie",
    "Charlee", "Ariya", "Alaya", "Jaylene", "Dulce",
    "Martha", "Pearl", "Jayda", "Kyra", "Kai",
    "Louisa", "Yamileth", "Simone", "Laney", "Luciana",
    "Kora", "Malaya", "Harlee", "Nala", "Dior",
    "Aleena", "Megan", "Maggie", "Ellis", "Jaylah",
    "Shiloh", "Sasha", "Jazlyn", "Madalyn", "Paris",
    "Laurel", "Sky", "Roselyn", "Adrianna", "Janelle",
    "Lia", "Anika", "Mara", "Meadow", "Aliza",
    "Zelda", "Leona", "Adele", "Kaydence", "Giana",
    "Yareli", "Kyla", "Braelynn", "Scarlet", "Skye",
    "Adelina", "Aimee", "Liana", "Reyna", "Saige",
    "Willa", "Amora", "Elianna", "Noa", "Zaniyah",
    "Belen", "Mavis", "Terri", "Milani", "Esme",
    "Lainey", "Oakley", "Hadassah", "Gwen", "Paulina",
    "Hunter", "Logan", "Sutton", "Ariyah"
};
    public string[] lastNames = {
    "Smith", "Johnson", "Williams", "Brown", "Jones",
    "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
    "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson",
    "Thomas", "Taylor", "Moore", "Jackson", "Martin",
    "Lee", "Perez", "Thompson", "White", "Harris",
    "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
    "Walker", "Young", "Allen", "King", "Wright",
    "Scott", "Torres", "Nguyen", "Hill", "Flores",
    "Green", "Adams", "Nelson", "Baker", "Hall",
    "Rivera", "Campbell", "Mitchell", "Carter", "Roberts",
    "Gomez", "Phillips", "Evans", "Turner", "Diaz",
    "Parker", "Cruz", "Edwards", "Collins", "Reyes",
    "Stewart", "Morris", "Morales", "Murphy", "Cook",
    "Rogers", "Gutierrez", "Ortiz", "Morgan", "Cooper",
    "Peterson", "Bailey", "Reed", "Kelly", "Howard",
    "Ramos", "Kim", "Cox", "Ward", "Richardson",
    "Watson", "Brooks", "Chavez", "Wood", "James",
    "Bennett", "Gray", "Mendoza", "Ruiz", "Hughes",
    "Price", "Alvarez", "Castillo", "Sanders", "Patel",
    "Myers", "Long", "Ross", "Foster", "Jimenez",
    "Powell", "Jenkins", "Perry", "Russell", "Sullivan",
    "Bell", "Coleman", "Butler", "Henderson", "Barnes",
    "Gonzales", "Fisher", "Vasquez", "Simmons", "Romero",
    "Jordan", "Patterson", "Alexander", "Hamilton", "Graham",
    "Reynolds", "Griffin", "Wallace", "Moreno", "West",
    "Cole", "Hayes", "Bryant", "Herrera", "Gibson",
    "Ellis", "Tran", "Medina", "Aguilar", "Stevens",
    "Murray", "Ford", "Castro", "Marshall", "Owens",
    "Harrison", "Fernandez", "McDonald", "Hawkins", "Mills",
    "Gomez", "Ryan", "Schmidt", "Carr", "Vasquez",
    "Castillo", "Wheeler", "Chapman", "Oliver", "Montgomery",
    "Richards", "Williamson", "Johnston", "Banks", "Meyer",
    "Bishop", "McCoy", "Howell", "Alvarez", "Morrison",
    "Hansen", "Fernandez", "Garza", "Harvey", "Little",
    "Burton", "Stanley", "Nguyen", "George", "Jacobs",
    "Reid", "Kim", "Fuller", "Lynch", "Dean",
    "Gilbert", "Garrett", "Romero", "Welch", "Larson",
    "Frazier", "Burke", "Hanson", "Day", "Mendoza",
    "Moreno", "Bowman", "Medina", "Fowler", "Brewer",
    "Hoffman", "Carlson", "Silva", "Pearson", "Holland",
    "Douglas", "Fleming", "Jensen", "Vargas", "Byrd",
    "Davidson", "Hopkins", "May", "Terry", "Herrera",
    "Wade", "Soto", "Walters", "Curtis", "Neal",
    "Caldwell", "Lowe", "Jennings", "Barnett", "Graves",
    "Jimenez", "Horton", "Shelton", "Barrett", "Obrien",
    "Castro", "Sutton", "Gregory", "McKinney", "Lucas",
    "Miles", "Craig", "Rodriquez", "Chambers", "Holt",
    "Lambert", "Fletcher", "Watts", "Bates", "Hale",
    "Rhodes", "Pena", "Beck", "Newman", "Haynes",
    "McDaniel", "Mendez", "Bush", "Vaughn", "Parks",
    "Dawson", "Santiago", "Norris", "Hardy", "Love",
    "Steele", "Curry", "Powers", "Schultz", "Barker",
    "Guzman", "Page", "Munoz", "Ball", "Keller",
    "Chandler", "Weber", "Leonard", "Walsh", "Lyons",
    "Ramsey", "Wolfe", "Schneider", "Mullins", "Benson",
    "Sharp", "Bowen", "Daniel", "Barber", "Cummings",
    "Hines", "Baldwin", "Griffith", "Valdez", "Hubbard",
    "Salazar", "Reeves", "Warner", "Stevenson", "Burgess",
    "Santos", "Tate", "Cross", "Garner", "Mann",
    "Mack", "Moss", "Thornton", "Dennis", "McGee",
    "Farmer", "Delgado", "Aguilar", "Vega", "Glover",
    "Manning", "Cohen", "Harmon", "Rodgers", "Robbins",
    "Newton", "Todd", "Blair", "Higgins", "Ingram",
    "Reese", "Cannon", "Strickland", "Townsend", "Potter",
    "Goodwin", "Walton", "Rowe", "Hampton", "Ortega",
    "Patton", "Swanson", "Joseph", "Francis", "Goodman",
    "Maldonado", "Yates", "Becker", "Erickson", "Hodges",
    "Rios", "Conner", "Adkins", "Webster", "Norman",
    "Malone", "Hammond", "Flowers", "Cobb", "Moody",
    "Quinn", "Blake", "Maxwell", "Pope", "Floyd",
    "Osborne", "Paul", "McCarthy", "Guerrero", "Lindsey",
    "Estrada", "Sandoval", "Gibbs", "Tyler", "Gross",
    "Fitzgerald", "Stokes", "Doyle", "Sherman", "Saunders",
    "Wise", "Colon", "Gill", "Alvarado", "Greer",
    "Padilla", "Simon", "Waters", "Nunez", "Ballard",
    "Schwartz", "McBride", "Houston", "Christensen", "Klein",
    "Pratt", "Briggs", "Parsons", "McLaughlin", "Zimmerman",
    "French", "Buchanan", "Moran", "Copeland", "Roy",
    "Pittman", "Brady", "McCormick", "Holloway", "Brock",
    "Poole", "Frank", "Logan", "Owen", "Bass",
    "Marsh", "Drake", "Wong", "Jefferson", "Park",
    "Morton", "Abbott", "Sparks", "Patrick", "Norton",
    "Huff", "Clayton", "Massey", "Lloyd", "Figueroa",
    "Carson", "Bowers", "Roberson", "Barton", "Tran",
    "Lamb", "Harrington", "Casey", "Boone", "Cortez",
    "Clarke", "Mathis", "Singleton", "Wilkins", "Cain",
    "Bryan", "Underwood", "Hogan", "McKenzie", "Collier",
    "Luna", "Phelps", "McGuire", "Allison", "Bridges",
    "Wilkerson", "Nash", "Summers", "Atkins", "Wilcox",
    "Pitts", "Conley", "Marquez", "Burnett", "Richard",
    "Cochran", "Chase", "Davenport", "Hood", "Gates",
    "Clay", "Ayala", "Sawyer", "Roman", "Vazquez",
    "Dickerson", "Hodge", "Acosta", "Flynn", "Espinoza",
    "Nicholson", "Monroe", "Wolf", "Morrow", "Kirk",
    "Randall", "Anthony", "Whitaker", "O'Connor", "Skinner",
    "Ware", "Molina", "Kirby", "Huffman", "Bradford",
    "Charles", "Gilmore", "Dominguez", "O'Neal", "Bruce",
    "Lang", "Combs", "Kramer", "Heath", "Hancock",
    "Gallagher", "Gaines", "Shaffer", "Short", "Wiggins",
    "Mathews", "McClain", "Fischer", "Wall", "Small",
    "Melton", "Hensley", "Bond", "Dyer", "Cameron",
    "Grimes", "Contreras", "Christian", "Wyatt", "Baxter",
    "Snow", "Mosley", "Shepherd", "Larsen", "Hoover",
    "Beasley", "Glenn", "Petersen", "Whitehead", "Meyers",
    "Keith", "Garrison", "Vincent", "Shields", "Horn",
    "Savage", "Olsen", "Schroeder", "Hartman", "Woodard",
    "Mueller", "Kemp", "Deleon", "Booth", "Patel",
    "Calhoun", "Wiley", "Eaton", "Cline", "Navarro",
    "Harrell", "Lester", "Humphrey", "Parrish", "Duran",
    "Hutchinson", "Hess", "Dorsey", "Bullock", "Robles",
    "Beard", "Dalton", "Avila", "Vance", "Rich",
    "Blackwell", "York", "Johns", "Blankenship", "Trevino",
    "Salinas", "Campos", "Pruitt", "Moses", "Callahan",
    "Golden", "Montoya", "Hardin", "Guerra", "McDowell",
    "Carey", "Stafford", "Gallegos", "Henson", "Wilkinson",
    "Booker", "Merritt", "Miranda", "Atkinson", "Orr",
    "Decker", "Hobbs", "Preston", "Tanner", "Knox",
    "Pacheco", "Stephenson", "Glass", "Rojas", "Serrano",
    "Marks", "Hickman", "English", "Sweeney", "Strong",
    "Prince", "McClure", "Conway", "Walter", "Roth",
    "Maynard", "Farrell", "Lowery", "Hurst", "Nixon",
    "Weiss", "Trujillo", "Ellison", "Sloan", "Juarez",
    "Winters", "McLean", "Randolph", "Leon", "Boyer",
    "Villarreal", "McCall", "Gentry", "Carrillo", "Kent",
    "Ayers", "Lara", "Shannon", "Sexton", "Pace",
    "Hull", "Leblanc", "Browning", "Velasquez", "Leach",
    "Chang", "House", "Bodine", "Wong", "Hogan",
    "Sharon", "Lionel", "Martell", "Ashford", "Leal",
    "Coelho", "Bianchi", "Samford", "Kern", "Duppstadt",
    "Zhang", "Densmore", "Burris", "Herald", "Thorne",
    "Swift", "Leong", "Milligan", "Eskridge", "Cordova",
    "Barlow", "Chalmers", "Worth", "Poindexter", "Bloom",
    "Starr", "Langley", "Purnell", "Mansfield", "Razo",
    "Boucher", "Fitch", "Albers", "Lin", "Setzer",
    "Morey", "Leighton", "Mahler", "Milton", "Langer",
    "Cushman", "Dell", "Noll", "Ingle", "Sage",
    "Ammons", "Bengtson", "Speer", "Eldridge", "Crandall",
    "Ching", "Dial", "Caudill", "Farnsworth", "Born",
    "Henley", "Tibbs", "Hollingsworth", "Hungerford", "Clare",
    "Gorman", "Yoder", "Heil", "Bowlin", "Perrin",
    "Haddad", "Haines", "Siegel", "McMullen", "Haskell",
    "Hawes", "Sills", "Stahl", "Currie", "Thayer",
    "Shultz", "Sherwood", "Macey", "Acton", "Greenfield",
    "Stroud", "Betz", "Oakes", "Thigpen", "Swank",
    "Jamison", "Teague", "Mohr", "Gallo", "Moriarty",
    "Peck", "Crary", "Elmore", "Rosen", "Beauchamp",
    "Pham", "Perdue", "Bradshaw", "Sorenson", "Heath",
    "Carranza", "Mott", "Hedrick", "Gallant", "Priest",
    "McAdams", "Lindquist", "Santoro", "Ahern", "Stapleton",
    "Christie", "Frey", "Voss", "Sturgeon", "Sisneros",
    "Paulson", "Braden", "Cromwell", "Broussard", "Moeller",
    "Rico", "Hildebrand", "Langston", "Guidry", "Ferreira",
    "Corley", "Conn", "Rossi", "Lackey", "Cody",
    "Estep", "Weir", "Ma", "Hartley", "Magoon",
    "Joiner", "Hatch", "Macias", "Lueck", "Goldfarb",
    "Patten", "Vick", "Steward", "Crain", "Stauffer",
    "Mayes", "Heim", "Whelan", "Wenzel", "Ventura",
    "Thrasher", "Manley", "Hilton", "Heinz", "Merrick",
    "Hurd", "Gustafson", "Deal", "Curran", "Ash",
    "Wardlaw", "Youmans", "Wildman", "Walrath", "Sprinkle",
    "Majors", "Abell", "Lipton", "Hathaway", "Sterling"
};
    public string[] hobbies = {
    "Painting",
    "Drawing",
    "Sketching",
    "Photography",
    "Knitting",
    "Crocheting",
    "Sewing",
    "Quilting",
    "Scrapbooking",
    "Gardening",
    "Fishing",
    "Hiking",
    "Camping",
    "Rock Climbing",
    "Swimming",
    "Cycling",
    "Running",
    "Jogging",
    "Yoga",
    "Meditation",
    "Cooking",
    "Baking",
    "Homebrewing",
    "Wine Making",
    "Playing Guitar",
    "Playing Piano",
    "Playing Drums",
    "Singing",
    "Dancing",
    "Writing",
    "Blogging",
    "Podcasting",
    "Vlogging",
    "Bird Watching",
    "Stargazing",
    "Astronomy",
    "Model Building",
    "Collecting Stamps",
    "Collecting Coins",
    "Collecting Cards",
    "Chess",
    "Board Games",
    "Video Gaming",
    "Puzzle Solving",
    "Magic Tricks",
    "Juggling",
    "Archery",
    "Shooting",
    "Horseback Riding",
    "Fencing",
    "Martial Arts",
    "Boxing",
    "Wrestling",
    "Bowling",
    "Golfing",
    "Tennis",
    "Table Tennis",
    "Badminton",
    "Skiing",
    "Snowboarding",
    "Surfing",
    "Skateboarding",
    "Rollerblading",
    "Scuba Diving",
    "Snorkeling",
    "Kite Flying",
    "Drone Flying",
    "Radio Control Cars",
    "Pottery",
    "Woodworking",
    "Metalworking",
    "Jewelry Making",
    "Origami",
    "Paper Mache",
    "Calligraphy",
    "Embroidery",
    "Cross-stitch",
    "Stamping",
    "Digital Art",
    "Graphic Design",
    "Animation",
    "3D Modeling",
    "Programming",
    "Web Development",
    "Learning Languages",
    "Traveling",
    "Geocaching",
    "Urban Exploration",
    "Aquarium Keeping",
    "Pet Training",
    "Dog Walking",
    "Falconry",
    "Tarot Reading",
    "Astrology",
    "Soap Making",
    "Candle Making",
    "Flower Arranging",
    "Social Dancing",
    "Ballroom Dancing",
    "Salsa Dancing",
    "Tango Dancing",
    "Belly Dancing",
    "Folk Dancing",
    "Creative Writing",
    "Poetry Writing",
    "Screenwriting",
    "Playwriting",
    "Novel Writing"
};
    public string[] cities = {
    "New York, NY",
    "Los Angeles, CA",
    "Chicago, IL",
    "Houston, TX",
    "Phoenix, AZ",
    "Philadelphia, PA",
    "San Antonio, TX",
    "San Diego, CA",
    "Dallas, TX",
    "San Jose, CA",
    "Austin, TX",
    "Jacksonville, FL",
    "San Francisco, CA",
    "Columbus, OH",
    "Fort Worth, TX",
    "Indianapolis, IN",
    "Charlotte, NC",
    "Seattle, WA",
    "Denver, CO",
    "Boston, MA",
    "El Paso, TX",
    "Nashville, TN",
    "Detroit, MI",
    "Oklahoma City, OK",
    "Portland, OR",
    "Las Vegas, NV",
    "Memphis, TN",
    "Louisville, KY",
    "Baltimore, MD",
    "Milwaukee, WI",
    "Albuquerque, NM",
    "Tucson, AZ",
    "Fresno, CA",
    "Mesa, AZ",
    "Sacramento, CA",
    "Atlanta, GA",
    "Kansas City, MO",
    "Colorado Springs, CO",
    "Omaha, NE",
    "Raleigh, NC",
    "Miami, FL",
    "Long Beach, CA",
    "Virginia Beach, VA",
    "Oakland, CA",
    "Minneapolis, MN",
    "Tulsa, OK",
    "Arlington, TX",
    "Tampa, FL",
    "New Orleans, LA",
    "Wichita, KS",
    "Cleveland, OH",
    "Bakersfield, CA",
    "Aurora, CO",
    "Anaheim, CA",
    "Honolulu, HI",
    "Santa Ana, CA",
    "Riverside, CA",
    "Corpus Christi, TX",
    "Lexington, KY",
    "Stockton, CA",
    "Henderson, NV",
    "Saint Paul, MN",
    "St. Louis, MO",
    "Cincinnati, OH",
    "Pittsburgh, PA",
    "Greensboro, NC",
    "Anchorage, AK",
    "Plano, TX",
    "Lincoln, NE",
    "Orlando, FL",
    "Irvine, CA",
    "Newark, NJ",
    "Toledo, OH",
    "Durham, NC",
    "Chula Vista, CA",
    "Fort Wayne, IN",
    "Jersey City, NJ",
    "St. Petersburg, FL",
    "Laredo, TX",
    "Madison, WI",
    "Chandler, AZ",
    "Buffalo, NY",
    "Lubbock, TX",
    "Scottsdale, AZ",
    "Reno, NV",
    "Glendale, AZ",
    "Gilbert, AZ",
    "Winston�Salem, NC",
    "North Las Vegas, NV",
    "Norfolk, VA",
    "Chesapeake, VA",
    "Garland, TX",
    "Irving, TX",
    "Hialeah, FL",
    "Fremont, CA",
    "Boise, ID",
    "Richmond, VA"
};
    public string[] companyNames = {
    "Quantum Solutions",
    "Nexa Innovations",
    "Vertex Dynamics",
    "Blue Ocean Enterprises",
    "Eclipse Industries",
    "Pinnacle Technology",
    "Vortex Systems",
    "Horizon Partners",
    "Nova Frontier",
    "Cosmo Corporation",
    "Summit Holdings",
    "Infinity Designs",
    "Momentum Incorporated",
    "Pulse Electronics",
    "Catalyst Creations",
    "Skyline Services",
    "Galaxy Builders",
    "Elemental Assets",
    "Ironclad Mechanics",
    "Streamline Productions",
    "Bridgeways Technologies",
    "Cloudscape Computing",
    "Futureworks Concepts",
    "Matrix Mechanics",
    "Neon Lighting",
    "Optima Networks",
    "Phoenix Endeavors",
    "Progression Engineering",
    "Radiant Systems",
    "Silverline Solutions",
    "Terra Firma Ltd.",
    "Urban Development Inc.",
    "Venture Visuals",
    "Zephyr Enterprises",
    "Alpha Streams",
    "Benchmark Business",
    "Creative Constructs",
    "Deep Dive Data",
    "Elevate Entertainment",
    "Flowstate Graphics",
    "Greenlight Growth",
    "Harmony Holdings",
    "Ideation Inc.",
    "Junction Geometrics",
    "Keystone Kinetics",
    "Limitless Labs",
    "Mosaic Manufacturing",
    "Noble Networks",
    "Orbit Outreach",
    "Progressive Properties",
    "Quasar Quality",
    "Rapid Results",
    "Strata Security",
    "Top Tier Tech",
    "Ultra Utilities",
    "Visionary Value",
    "Wildcard Webworks",
    "X-factor Xerography",
    "Yieldmax Yachting",
    "Zenith Zones",
    "Acclaim Agencies",
    "Biotech Beacon",
    "Chronicle Creations",
    "Dimensional Data",
    "Echo Eco",
    "Fusion Facilities",
    "Gridlock Games",
    "Haven Homes",
    "Impulse Ideas",
    "Journeyman Jewels",
    "Karma Kinetics",
    "Legacy Lending",
    "Majestic Markets",
    "New Age Navigators",
    "Omni Operations",
    "Paragon Projects",
    "Quantech Quality",
    "Renaissance Retail",
    "Stellar Supplies",
    "Titan Textiles",
    "Unity Utilities",
    "Valor Ventures",
    "Wingspan Works",
    "Xenon Xtreme",
    "Yellowstone Yield",
    "Zodiac Zones",
    "Astra Agency",
    "Beacon Business",
    "Crestview Concepts",
    "Digital Domain",
    "Elite Enterprises",
    "Frontier Functions",
    "Giga Guild",
    "Highrise Horizons",
    "Innova Insights",
    "Jolt Journey",
    "Kaleidoscope Keys",
    "Lunar Landscapes",
    "Meridian Markets",
    "Nimbus Networks",
    "Oasis Operations",
    "Prime Prospects",
    "Quest Quotient",
    "Ridgeview Resources",
    "Satellite Strategies",
    "Terracotta Trades",
    "Umbra Universe",
    "Vivid Ventures",
    "Wavefront Works",
    "Expanse Experts",
    "Yellowroad Yarns",
    "Zestful Zenith"
};
    public string[] contractName = {
    "Apple",
    "Banana Bunch",
    "Cat",
    "Dog",
    "Elephant",
    "Flower",
    "Grapes",
    "House",
    "Ice Cream Cone",
    "Jellyfish",
    "Kite",
    "Ladybug",
    "Moon",
    "Nest",
    "Orange Slice",
    "Palm Tree",
    "Quill",
    "Rose",
    "Sunrise",
    "Tree",
    "Umbrella",
    "Violin",
    "Watermelon",
    "Xylophone",
    "Yacht",
    "Zebra",
    "Beach Ball",
    "Cupcake",
    "Dolphin",
    "Eagle",
    "Fish",
    "Guitar",
    "Hat",
    "Iceberg",
    "Jug",
    "Kangaroo",
    "Leaf",
    "Mushroom",
    "Notebook",
    "Owl",
    "Pumpkin",
    "Quartz Crystal",
    "Rainbow",
    "Star",
    "Train",
    "Unicorn",
    "Vase",
    "Wagon",
    "X-ray Fish",
    "Yellow Jacket",
    "Zucchini",
    "Ant",
    "Bicycle",
    "Chair",
    "Desk",
    "Envelope",
    "Frog",
    "Giraffe",
    "Helicopter",
    "Island",
    "Juice Box",
    "Key",
    "Lamp",
    "Mountain",
    "Necktie",
    "Octopus",
    "Pencil",
    "Queen Bee",
    "Robot",
    "Snake",
    "Tulip",
    "Urchin",
    "Van",
    "Windmill",
    "Xenops Bird",
    "Yarn",
    "Zigzag Pattern",
    "Acorn",
    "Bridge",
    "Cloud",
    "Daisy",
    "Egg",
    "Feather",
    "Glass",
    "Heart",
    "Igloo",
    "Jack-o'-lantern",
    "Kiwi",
    "Lighthouse",
    "Maple Leaf",
    "Night Sky",
    "Ostrich",
    "Pine Tree",
    "Quail",
    "Racquet",
    "Sailboat",
    "Teapot",
    "Ukulele",
    "Volcano",
    "Wheelbarrow",
    "Xerus",
    "Yellow Bell Pepper",
    "Zigzag Road"
};


    public string[] factoryJobs = {"N/A","Harvest Engineer", "Machine Operator", "Quality Inspector", "Forklift Driver", "Maintenance Technician", "Welder", "Packager", "Material Handler", "Production Supervisor", "Safety Manager" };
    public string[] employmentStatuses = { "Full-Time", "Part-Time", "Temporary", "Contract", "Intern", "Freelance", "Remote", "On Leave", "Unemployed", "Retired" };
    public string[] employeeStatus = {"idle","working"};
    public string[] machineStatuses = { "IDLE", "Loading", "RUNNING", "Unloading", "Completed", "Incomplete","IDLE", "Faulted", "Blocked", "Starved", "Changeover", "Maintenace", "Available", "Ready" };
    public string[] factoryEvents = { "Machine Malfunction", "Safety Inspection", "Inventory Restock", "Employee Training", "Quality Control Audit", "Shift Change", "Power Outage", "Scheduled Maintenance", "Product Launch", "Emergency Drill" };
    public string[] eventStatuses = { "", "Red Chosen!", "Green Chosen!", "Blue Chosen!", "Job is processed", "Machine is Running", "Random Event/Flavor Text", "Machine is Starved", "harvesting...", "Upgrade Harvest Capacity", "Harvester at capacity", "Maintenance", "Refinery Started", "Seasonal", "System","Production Idle","Pay Employees","Pay Rent"};
    public string[] woStatuses = {"N/A","In Queue","Loading","In Production","Unloading","Completed","Incomplete",""};
    public string[] zodiacSigns = {"N/A","Aries","Taurus","Gemini","Cancer","Leo","Virgo","Libra","Scorpio","Sagittarius","Capricorn","Aquarius","Pisces"};



    System.Random r = new System.Random();

    public int currentYear = DateTime.Now.Year;
    public string generateMachineName()
    {

        // Define the characters and numbers to be used in each segment
        string segment1Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string segment2Numbers = "0123456789";
        string segment3Chars = "abcdefghijklmnopqrstuvwxyz";

        // Initialize the machine machineName as an empty string
        String machineName = "";

        // Generate the first segment (e.g., "TJZ")
        for (int i = 0; i < 3; i++)
        {
            char randomChar = segment1Chars[UnityEngine.Random.Range(0, segment1Chars.Length)];
            machineName += randomChar;
        }

        // Add a dash
        machineName += "-";

        // Generate the second segment (e.g., "523")
        for (int i = 0; i < 3; i++)
        {
            char randomNum = segment2Numbers[UnityEngine.Random.Range(0, segment2Numbers.Length)];
            machineName += randomNum;
        }

        // Add another dash
        machineName += "-";

        // Generate the third segment (e.g., "j42")
        char randomChar3 = segment3Chars[UnityEngine.Random.Range(0, segment3Chars.Length)];
        machineName += randomChar3;
        for (int i = 0; i < 2; i++)
        {
            char randomNum = segment2Numbers[UnityEngine.Random.Range(0, segment2Numbers.Length)];
            machineName += randomNum;
        }
        return machineName;
    }

    public string orderName()
    {
        string segment1Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        String machineName = "";

        // Generate the first segment (e.g., "TJZ")
        for (int i = 0; i < 3; i++)
        {
            char randomChar = segment1Chars[UnityEngine.Random.Range(0, segment1Chars.Length)];
            machineName += randomChar;
        }

        return machineName;
    }

    public int rollDice(int dice, int sides)  //rolls x dice of 1 to max
    {
        int roll = 0;

        for (int i = 0; i < dice; i++)
        {
            roll += r.Next(1, sides + 1);
        }

        return roll;
    }

    public bool skillCheck(int skill)
    {
        if (rollDice(1, 2) > skill)
            return false;
        return true;
    }
    public DateTime GenerateRandomDate(int year)
    {
        System.Random random = new System.Random();
        int month = random.Next(1, 13); // Random month
        int day;

        // Handle February separately because of leap year
        if (month == 2)
        {
            if (DateTime.IsLeapYear(year))
            {
                day = random.Next(1, 30); // 1 to 29 for leap year
            }
            else
            {
                day = random.Next(1, 29); // 1 to 28 for non-leap year
            }
        }
        else if (month == 4 || month == 6 || month == 9 || month == 11)
        {
            day = random.Next(1, 31); // 1 to 30 for months with 30 days
        }
        else
        {
            day = random.Next(1, 32); // 1 to 31 for months with 31 days
        }

        return new DateTime(year, month, day);
    }

    public int highestDice(int dice, int sides)
    {
        int highest=0;
        int count = 1;

        for(int i = 0;i<dice;i++)
            if (sides >= highest)
            {
                if (sides == highest && highest == 20)
                    count++;

                highest = sides;
            }

        return highest * count;

    }

    public int rarity()
    {
        int roll = rollDice(1, 100);

        if (roll > 90)
            return 3;
        else if (roll > 60)
            return 2;
        else 
            return 1;


    }
}
