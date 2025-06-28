namespace StationeryAndSuppliesWebApp.Models; 

public class UserReviewsList
{
    public List<UserReview> UserReviews { get; set; } = new List<UserReview>();

    public double AverageScore
    {
        get 
        {
            int totalReviews = UserReviews.Count;

            double sumOfReviews = UserReviews.Sum(r => r.Rating); 

            double averageScore = Math.Round(sumOfReviews / totalReviews, 1);

            return averageScore; 
        }
    }

    public int TotalReviews => UserReviews.Count;
}
