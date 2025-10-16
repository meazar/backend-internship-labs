using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterRegistrationSystem.Interfaces;
using VoterRegistrationSystem.Models;
using VoterRegistrationSystem.Exceptions;
using System.Collections.Generic;

namespace VoterRegistrationSystem.Services
{
    public class VoterService: IVoterService
    {
        //sabai register vako voter haru lai order ma rakhxa
        private readonly List<Voter> voters = new List<Voter>();
        //citizenshipid lai map garxa so that we can lookup voter fast
        private readonly Dictionary<string, Voter> voterDictionary = new Dictionary<string, Voter>();

        public void RegisterVoter(Voter voter)
        {
            try
            {
                if(voter.Age < 18)
                {
                    throw new InvalidAgeException("Voter must be At least 18 years old.");
                }
                //citizenship Id duplicate check garna use hunxa 
                if (voterDictionary.ContainsKey(voter.CitizenshipId))
                {
                    Console.WriteLine("Voter with this citizenship ID already exists.");
                    return;
                }

                //each voter lai naya viterid banaidinxa ie. V001,V002
                voter.VoterId = $"V{voters.Count + 1:000}";
                voters.Add(voter);
                voterDictionary[voter.CitizenshipId] = voter;
                Console.WriteLine("Voter registered successfully");
               
            }
            catch(InvalidAgeException ex)
            {
                Console.WriteLine($"Unexpected error : {ex.Message}");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexcepted error : {ex.Message}");
            }
        }

        public List<Voter> GetAllVoters()
        {
            return voters;
        }

        public Voter SearchByCitizenship(string citizenshipId)
        {
            //TryGEtValue le chai ID exist garxa ki gadaina check garxa in dictionary
            voterDictionary.TryGetValue(citizenshipId, out Voter voter);
            return voter;
        }
    }
}
