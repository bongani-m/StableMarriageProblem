using System;
using System.Collections.Generic;
using System.Text;

namespace StableMarriageProblem
{
    class Org : UserEntity
    {
        public string Name { get; set; }
        public List<User> Members { get; set; }

    }
}
