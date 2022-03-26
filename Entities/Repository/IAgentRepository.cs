﻿using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Entities.Repository
{
    public interface IAgentRepository
    {
        public void CreateAgent(Agent agent);
        public void UpdateAgent(Agent agent);
        public Task<IEnumerable<Agent>> GetAllAgents();
        public Task<Agent> GetAgent(int Id);
        public Task<int> SaveChangesAsync();
    }
}