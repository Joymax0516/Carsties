/** @type {import('next').NextConfig} */
const nextConfig = {
  experimental : {
    serverAction: true
  },
  images:{
    domains: [
      'cdn.pixabay.com'
    ]
  }
};

export default nextConfig;
