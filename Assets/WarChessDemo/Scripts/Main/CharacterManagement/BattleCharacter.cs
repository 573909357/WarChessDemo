using System.Collections.Generic;
using UnityEngine;

public class BattleCharacter
{
    public CharacterSetup setup;
    public int health;
    public int attack;
    public Vector3Int position;
    public bool isEnemy;
    public bool moved;
    
    public BattleCharacter(CharacterSetup setup, bool isEnemy)
    {
        this.setup = setup;
        this.isEnemy = isEnemy;
        health = setup.maxHealth;
        attack = setup.attack;
        position = Vector3Int.zero;
        moved = false;
    }

    public void Move(Vector3Int position)
    {
        this.position = position;
        moved = true;
        GameInstance.gameInstance.battlePanel.UpdateView();
    }

    public void Attack(Vector3Int position)
    {
        BattleCharacter target = null;
        if (isEnemy == false)
        {
            target = GameInstance.gameInstance.battleManager.GetEnemy(position);
            if (target == null)
                return;
        }
        else 
        {
            target = GameInstance.gameInstance.battleManager.GetPlayerCharacter(position);
            if (target == null)
                return;
        }
    
        target.health -= attack;
        if (target.health <= 0)
        {
            GameInstance.gameInstance.battleManager.Kill(target);
        }
        else
        {
            GameInstance.gameInstance.battlePanel.UpdateView();
        }
    }

    public void EnemyMove()
    {
        var positions = GetCanMovePositions();
        var shortest = int.MaxValue;
        Vector3Int from = Vector3Int.zero, to= Vector3Int.zero;
        foreach (var position in positions)
        {
            foreach (var character in GameInstance.gameInstance.battleManager.characters)
            {
                foreach (var beAttackedPosition in character.GetBeAttackedPositions())
                {
                    var distance = GetDistance(position, beAttackedPosition);
                    if (shortest <= distance)
                        continue;
                    shortest = distance;
                    from = position;
                    to = beAttackedPosition;
                }
            }
        }

        Move(from);
    }
    
    public bool EnemyAttack()
    {
        foreach (var position in GetCanAttackPositions())
        {
            var character = GameInstance.gameInstance.battleManager.GetPlayerCharacter(position);
            if (character != null)
            {
                Attack(position);
                return true;
            }
        }
        return false;
    }
    
    public List<Vector3Int> GetCanMovePositions()
    {
        var result = new List<Vector3Int>();
        var currentPath = new Queue<PathInfo>();
        var battleManager = GameInstance.gameInstance.battleManager;
        
        result.Add(position);
        currentPath.Enqueue(new PathInfo{position = position, depth = 0});
        
        void Compute(Vector3Int position, int depth)
        {
            if (battleManager.levelSetup.cells.ContainsKey(position) && battleManager.levelSetup.cells[position].canMove)
            {
                result.Add(position);
                currentPath.Enqueue(new PathInfo{position = position, depth = depth + 1});
            }
        }
        
        while (currentPath.Count > 0)
        {
            var current = currentPath.Dequeue();
            if (current.depth >= setup.move)
                continue;
            
            Compute(current.position + Vector3Int.up, current.depth);
            Compute(current.position + Vector3Int.left, current.depth);
            Compute(current.position + Vector3Int.right, current.depth);
            Compute(current.position + Vector3Int.down, current.depth);
        }
        
        return result;
    }
    
    public List<Vector3Int> GetCanAttackPositions()
    {
        var result = new List<Vector3Int>();
        var battleManager = GameInstance.gameInstance.battleManager;

        void Compute(Vector3Int position)
        {
            if (battleManager.levelSetup.cells.ContainsKey(position) && battleManager.levelSetup.cells[position].canMove)
            {
                result.Add(position);
            }
        }

        Compute(position + Vector3Int.up);
        Compute(position + Vector3Int.left);
        Compute(position + Vector3Int.right);
        Compute(position + Vector3Int.down);
        
        return result;
    }

    public List<Vector3Int> GetBeAttackedPositions()
    {
        var result = new List<Vector3Int>();
        var battleManager = GameInstance.gameInstance.battleManager;

        void Compute(Vector3Int position)
        {
            if (battleManager.levelSetup.cells.ContainsKey(position) && battleManager.levelSetup.cells[position].canMove)
            {
                result.Add(position);
            }
        }

        Compute(position + Vector3Int.up);
        Compute(position + Vector3Int.left);
        Compute(position + Vector3Int.right);
        Compute(position + Vector3Int.down);
        
        return result;
    }
    
    public int GetDistance(Vector3Int from, Vector3Int to)
    {        
        if (from == to)
            return 0;
        var depths = new List<Vector3Int>();
        var currentPath = new Queue<PathInfo>();
        var battleManager = GameInstance.gameInstance.battleManager;
        var shortest = int.MaxValue;
        
        currentPath.Enqueue(new PathInfo{position = from, depth = 0});
        
        void Compute(Vector3Int position, int depth)
        {
            if (battleManager.levelSetup.cells.ContainsKey(position) && battleManager.levelSetup.cells[position].canMove)
            {
                if (to == position)
                {
                    if (shortest <= depth + 1)
                        return;
                    shortest = depth + 1;
                    return;
                }

                if (depths.Contains(position))
                    return;
                
                depths.Add(position);
                currentPath.Enqueue(new PathInfo{position = position, depth = depth + 1});
            }
        }
        
        while (currentPath.Count > 0)
        {
            var current = currentPath.Dequeue();
            Compute(current.position + Vector3Int.up, current.depth);
            Compute(current.position + Vector3Int.left, current.depth);
            Compute(current.position + Vector3Int.right, current.depth);
            Compute(current.position + Vector3Int.down, current.depth);
        }
        
        return shortest;
    }
    
    public class PathInfo
    {
        public Vector3Int position;
        public int depth;
    }
}
