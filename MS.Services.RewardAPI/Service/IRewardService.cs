using MS.Services.RewardAPI.Message;

namespace MS.Services.RewardAPI.Service
{
    public interface IRewardService
    {
        Task UpdateRewards(RewardsMessage rewardsMessage);
    }
}
