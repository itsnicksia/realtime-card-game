aws s3 sync dist/Web s3://cards.fasterthoughts.io/ --delete
aws s3 cp dist/Web s3://cards.fasterthoughts.io/ --exclude="*" --include="*.gz" --content-encoding gzip --content-type="binary/octet-stream" --metadata-directive REPLACE --recursive
aws s3 cp dist/Web s3://cards.fasterthoughts.io/ --exclude="*" --include="*.js.gz" --content-encoding gzip --content-type="application/javascript" --metadata-directive REPLACE --recursive
aws s3 cp dist/Web s3://cards.fasterthoughts.io/ --exclude="*" --include="*.wasm.gz" --content-encoding gzip --content-type="application/wasm" --metadata-directive REPLACE --recursive
aws cloudfront create-invalidation --distribution-id E31A0AUI49SALI --paths "/*"
