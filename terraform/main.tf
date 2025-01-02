terraform {
  cloud {
    organization = "fasterthoughts"
    workspaces {
      name = "realtime-card-game"
    }
  }
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
}

locals {
  route53_zone_id = "Z1MO7VGBJ38IIB"
}

provider "aws" {
  region = "ap-southeast-2"
}

resource "aws_s3_bucket" "website_bucket" {
  bucket = "cards.fasterthoughts.io"
}

resource "aws_s3_bucket_public_access_block" "website_public_access_block" {
  bucket = aws_s3_bucket.website_bucket.id

  block_public_acls       = false
  block_public_policy     = false
  ignore_public_acls      = false
  restrict_public_buckets = false
}

resource "aws_s3_bucket_website_configuration" "website_config" {
  bucket = aws_s3_bucket.website_bucket.id
  index_document {
    suffix = "index.html"
  }
  error_document {
    key = "index.html"
  }
}

resource "aws_s3_bucket_policy" "website_policy" {
  bucket = aws_s3_bucket.website_bucket.id

  policy = jsonencode({
    Version   = "2012-10-17",
    Statement = [
      {
        Effect    = "Allow",
        Principal = "*",
        Action    = "s3:GetObject",
        Resource  = "${aws_s3_bucket.website_bucket.arn}/*"
      }
    ]
  })
}

resource "aws_s3_bucket_cors_configuration" "website_cors" {
  bucket = aws_s3_bucket.website_bucket.id

  cors_rule {
    allowed_methods = ["GET"]
    allowed_origins = ["*"]
    allowed_headers = ["*", "Content-Type"]
    expose_headers  = ["ETag"]
    max_age_seconds = 3000
  }
}

# Example HTML files to upload
resource "local_file" "index_html" {
  content  = <<EOT
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Hello World</title>
</head>
<body>
    <h1>Hello, World!</h1>
    <p>Welcome to your Unity WebGL app hosted on S3.</p>
</body>
</html>
EOT
  filename = "index.html"
}

resource "local_file" "error_html" {
  content  = <<EOT
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Error</title>
</head>
<body>
    <h1>Oops!</h1>
    <p>Something went wrong. The page you are looking for could not be found.</p>
</body>
</html>
EOT
  filename = "error.html"
}

resource "aws_s3_object" "index_html_upload" {
  bucket       = aws_s3_bucket.website_bucket.id
  key          = "index.html"
  source       = local_file.index_html.filename
  content_type = "text/html"
}

resource "aws_s3_object" "error_html_upload" {
  bucket       = aws_s3_bucket.website_bucket.id
  key          = "error.html"
  source       = local_file.error_html.filename
  content_type = "text/html"
}

resource "aws_route53_record" "s3_alias" {
  zone_id = local.route53_zone_id
  name    = "cards.fasterthoughts.io" # Replace with your subdomain or root domain
  type    = "A"

  alias {
    name                   = aws_s3_bucket.website_bucket.website_domain
    zone_id                = aws_s3_bucket.website_bucket.hosted_zone_id
    evaluate_target_health = false
  }
}

resource "aws_iam_user" "s3_sync_user" {
  name = "s3-sync-user"
}

resource "aws_iam_policy_attachment" "s3_sync_attachment" {
  name       = "card-game-s3-sync-attachment"
  users      = [aws_iam_user.s3_sync_user.name]
  policy_arn = aws_iam_policy.s3_sync_policy.arn
}

resource "aws_iam_policy" "s3_sync_policy" {
  name        = "s3-sync-policy"
  description = "Policy to allow S3 sync operations"

  policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Effect   = "Allow",
        Action   = ["s3:PutObject", "s3:ListBucket", "s3:DeleteObject"],
        Resource = [
          aws_s3_bucket.website_bucket.arn,
          "${aws_s3_bucket.website_bucket.arn}/*"
        ]
      }
    ]
  })
}

output "bucket_website_endpoint" {
  value = aws_s3_bucket_website_configuration.website_config.website_endpoint
}

output "bucket_website_domain" {
  value = aws_s3_bucket_website_configuration.website_config.website_domain
}
