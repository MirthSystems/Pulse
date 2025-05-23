import { useState } from "react";
import { useNavigate } from "react-router";
import { Input, Button } from "@heroui/react";
import { SearchIcon } from "@/components/icons";
import { title } from "@/components/primitives";

export default function IndexPage() {
  const navigate = useNavigate();
  const [searchQuery, setSearchQuery] = useState("");

  const handleSearch = () => {
    if (searchQuery.trim()) {
      navigate(`/search?q=${encodeURIComponent(searchQuery)}`);
    }
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === "Enter") {
      handleSearch();
    }
  };  return (
    <section className="flex flex-col items-center justify-center gap-4 py-8 md:py-10">
      <div className="inline-block max-w-lg text-center justify-center">
        <span className={title()}>Welcome to&nbsp;</span>
        <span className={title({ color: "violet" })}>Pulse&nbsp;</span>
        <br />
        <span className={title()}>
          Your search starts here.
        </span>
      </div>

      <div className="w-full max-w-md flex gap-2 mt-8">
        <Input
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
          onKeyPress={handleKeyPress}
          placeholder="Search..."
          startContent={<SearchIcon className="text-base text-default-400 pointer-events-none flex-shrink-0" />}
          className="flex-1"
          size="lg"
        />
        <Button color="primary" onPress={handleSearch} size="lg">
          Search
        </Button>
      </div>
    </section>
  );
}
