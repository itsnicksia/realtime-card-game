resource "aws_iam_user" "s3_sync_user" {
  name = "s3-sync-user"
}

resource "aws_iam_policy_attachment" "s3_sync_attachment" {
  name       = "card-game-s3-sync-attachment"
  users      = [aws_iam_user.s3_sync_user.name]
  policy_arn = aws_iam_policy.s3_sync_policy.arn
}
