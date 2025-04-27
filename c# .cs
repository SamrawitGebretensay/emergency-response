using System;
using System.Collections.Generic;
using System.Linq;

// Define the abstract base class for emergency units
public abstract class EmergencyUnit
{
    // Properties: Name and Speed
    public string Name { get; set; }
    public int Speed { get; set; }

    // Constructor for the EmergencyUnit class
    public EmergencyUnit(string name, int speed)
    {
        Name = name;
        Speed = speed;
    }

    // Abstract method to check if the unit can handle a specific incident type
    public abstract bool CanHandle(string incidentType);

    // Abstract method to simulate the unit's response to an incident
    public abstract void RespondToIncident(Incident incident);
}

// Define the Police class, which inherits from EmergencyUnit
public class Police : EmergencyUnit
{
    // Constructor for the Police class
    public Police(string name, int speed) : base(name, speed) { }

    // Override the CanHandle method to specify that Police units handle "Crime" incidents
    public override bool CanHandle(string incidentType)
    {
        return incidentType.ToLower() == "crime"; // Case-insensitive comparison
    }

    // Override the RespondToIncident method to simulate how a Police unit responds
    public override void RespondToIncident(Incident incident)
    {
        Console.WriteLine($"{Name} is responding to a crime at {incident.Location}. Officer on scene.");
        Console.WriteLine($"Investigating the situation.");
        Console.WriteLine($"Suspect apprehended."); // Added more details to the response.
    }
}

// Define the Firefighter class, which inherits from EmergencyUnit
public class Firefighter : EmergencyUnit
{
    // Constructor for the Firefighter class
    public Firefighter(string name, int speed) : base(name, speed) { }

    // Override the CanHandle method to specify that Firefighter units handle "Fire" incidents
    public override bool CanHandle(string incidentType)
    {
        return incidentType.ToLower() == "fire"; // Case-insensitive comparison
    }

    // Override the RespondToIncident method to simulate how a Firefighter unit responds
    public override void RespondToIncident(Incident incident)
    {
        Console.WriteLine($"{Name} is responding to a fire at {incident.Location}. Firefighters arriving.");
        Console.WriteLine($"Locating the source of the fire.");
        Console.WriteLine($"Fire extinguished."); // Added more details
    }
}

// Define the Ambulance class, which inherits from EmergencyUnit
public class Ambulance : EmergencyUnit
{
    // Constructor for the Ambulance class
    public Ambulance(string name, int speed) : base(name, speed) { }

    // Override the CanHandle method to specify that Ambulance units handle "Medical" incidents
    public override bool CanHandle(string incidentType)
    {
        return incidentType.ToLower() == "medical"; // Case-insensitive comparison
    }

    // Override the RespondToIncident method to simulate how an Ambulance unit responds
    public override void RespondToIncident(Incident incident)
    {
        Console.WriteLine($"{Name} is responding to a medical emergency at {incident.Location}. Paramedics en route.");
        Console.WriteLine($"Assessing the patient's condition.");
        Console.WriteLine($"Patient stabilized and transported to hospital."); // More details.
    }
}

// Define the Incident class to represent an emergency event
public class Incident
{
    // Properties: Type and Location
    public string Type { get; set; }
    public string Location { get; set; }

    // Constructor for the Incident class
    public Incident(string type, string location)
    {
        Type = type;
        Location = location;
    }
}

public class Simulation
{
    // List to store available emergency units
    private static List<EmergencyUnit> _emergencyUnits;
    // List of possible incident types
    private static List<string> _incidentTypes = new List<string> { "Crime", "Fire", "Medical" };
    // Random number generator for simulating random events
    private static Random _random = new Random();

    // Main method: Entry point of the program
    public static void Main(string[] args)
    {
        // Initialize the emergency units
        _emergencyUnits = new List<EmergencyUnit>
        {
            new Police("Police Unit 1", 60),
            new Police("Police Unit 2", 50),
            new Firefighter("Fire Truck 1", 45),
            new Firefighter("Fire Truck 2", 55),
            new Ambulance("Ambulance 1", 70),
            new Ambulance("Ambulance 2", 65)
        };

        // Run the simulation for a specified number of rounds
        int numberOfRounds = 5;
        int score = 0; // Keep track of the score.

        for (int round = 1; round <= numberOfRounds; round++)
        {
            Console.WriteLine($"\n--- Round {round} ---");

            // Get incident details from the user
            Incident currentIncident = GetIncidentFromUser();

            // Find the most suitable unit to respond to the incident
            EmergencyUnit respondingUnit = FindBestAvailableUnit(currentIncident.Type);

            // Dispatch the unit and display the response
            if (respondingUnit != null)
            {
                respondingUnit.RespondToIncident(currentIncident);
                score += 10; // Award points for a successful response.
                Console.WriteLine($"Response successful! Unit dispatched: {respondingUnit.Name}");
                // Remove the unit *after* it has responded.
                _emergencyUnits.Remove(respondingUnit);
            }
            else
            {
                Console.WriteLine($"No suitable unit available to handle the {currentIncident.Type} incident.");
            }
            Console.WriteLine($"Current Score: {score}");
            Console.WriteLine("Press Enter to continue..."); // Pause for user input
            Console.ReadLine();
        }
        Console.WriteLine("\n--- Simulation Ended ---");
        Console.WriteLine($"Final Score: {score}");
        Console.WriteLine("Thank you for playing!");
    }

    // Method to get incident details from the user, with number or character selection
    private static Incident GetIncidentFromUser()
    {
        string incidentType = "";
        string incidentLocation = "";

        Console.WriteLine("Available incident types:");
        for (int i = 0; i < _incidentTypes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_incidentTypes[i]}");
        }
        Console.WriteLine($"Or type the incident type directly (e.g., Crime, Fire, Medical).");

        // Get incident type from user using a number or character
        while (true)
        {
            Console.Write("Enter the number or name of the incident type: ");
            string input = Console.ReadLine()?.Trim();

            // Try to parse as a number
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= _incidentTypes.Count)
            {
                incidentType = _incidentTypes[choice - 1];
                break; // Exit the loop if number input is valid
            }
            // Otherwise, treat it as a string
            else if (!string.IsNullOrEmpty(input))
            {
                incidentType = input;
                break; // Exit the loop if string input is not empty
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number from the list or the incident type name.");
            }
        }

        // Get incident location from user
        Console.Write("Enter incident location: ");
        incidentLocation = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(incidentLocation))
        {
            Console.WriteLine("Location cannot be empty. Using default location 'Unknown'.");
            incidentLocation = "Unknown"; // Set a default location
        }

        return new Incident(incidentType, incidentLocation);
    }

    // Method to find the most suitable available unit for a given incident type
    private static EmergencyUnit FindBestAvailableUnit(string incidentType)
    {
        // Find units that can handle the incident (case-insensitive).
        List<EmergencyUnit> availableUnits = _emergencyUnits
            .Where(unit => unit.CanHandle(incidentType))
            .ToList();

        if (availableUnits.Count == 0)
        {
            return null; // No units available.
        }
        // Find the fastest unit among the available units.
        EmergencyUnit bestUnit = availableUnits.OrderBy(unit => unit.Speed).FirstOrDefault();

        return bestUnit;
    }
}
