namespace CallingBotSample.Model
{
  public class CallDetails
  {
    /// <summary>
    /// Gets or sets the name of participant.
    /// </summary>
    public string CallId { get; set; }
    public string ParticipantId { get; set; }
    public string ParticipantName { get; set; }
    public string State { get; set; }
  }
}
