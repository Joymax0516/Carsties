import Listings from "./auctions/Listings";

export default function Home()
{
  console.log('Server component');
  return (
    <div>
      <h3 className = "text-3xl font-semibold">
        <Listings/>
      </h3>
    </div>
  )
}