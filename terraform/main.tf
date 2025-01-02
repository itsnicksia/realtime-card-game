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

data "aws_caller_identity" "current" {}

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

output "bucket_website_endpoint" {
  value = aws_s3_bucket_website_configuration.website_config.website_endpoint
}

output "bucket_website_domain" {
  value = aws_s3_bucket_website_configuration.website_config.website_domain
}
