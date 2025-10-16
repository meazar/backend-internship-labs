using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterRegistrationSystem.Models;

namespace VoterRegistrationSystem.Interfaces
{
    public interface IVoterService
    {
        void RegisterVoter(Voter voter);
        List<Voter> GetAllVoters();
        Voter SearchByCitizenship(string citizenshipId);
    }
}
