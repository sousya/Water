using UnityEngine;
using WaterGame.Models;

namespace WaterGame.Controllers
{
    public interface IBottleController
    {
        void Initialize(BottleState state);
        void Select();
        void Deselect();
        bool CanMoveWater(IBottleController target);
        void MoveWater(IBottleController target);
        void UpdateVisuals();
        void AddWater(Water water);
        void RemoveTopWater();
        Water GetTopWater();
        bool IsEmpty();
        bool IsFull();
        bool IsFrozen();
        bool IsHidden();
        bool IsClearHidden();
        bool IsNearHidden();
        bool IsFinished();
        Transform GetWaterTransform();
        Transform GetBottleTransform();
        int GetBottleIndex();
        void SetFrozen(bool frozen);
        void SetHidden(bool hidden);
        void SetClearHidden(bool clearHidden);
        void SetNearHidden(bool nearHidden);
        void SetFinished(bool finished);
        void CheckFinish();
    }
} 