﻿using Memoria.Battle.Managers;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.Events
{
    public class NewTurn : GameEvent
    {
        public Entity entity;
        public bool curve;
        public bool moved;

        public NewTurn(Entity e, bool moves, bool curve)
        {
            this.entity = e;
            this.curve = curve;
            this.moved = moves;
        }
    }
}